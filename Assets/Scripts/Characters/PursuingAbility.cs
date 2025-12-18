using NUnit.Framework;  
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class PursuingAbility : MonoBehaviour
{
    [SerializeField] private LayerMask platformMask;

    private List<GameObject> players;
    private GameObject targetPlayer;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private State currentState;
    private Vector3 baseScale;
    private PlayerController targetPlayerController;
    private Animator anim;
    private List<GameObject> groundedPlayers;
    private float stepSize = 0.5f;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float minDistanceToTeleportFromPlayer = 2.0f;
    [SerializeField] private float jumpInDuration = 1.0f;
    [SerializeField] private float jumpOutDuration = 1.0f;
    private enum State
    {
        Stunned,
        ChaseTarget,
        FindTarget,
        JumpIn,
        JumpOut
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        players = new List<GameObject>();
        foreach (var character in CharacterManager.Instance.Characters)
        {
            if (character.instance != null)
                players.Add(character.instance);
        }
        currentState = State.FindTarget;
        baseScale = transform.localScale;
    }

    void SetTarget(GameObject target)
    {
        targetPlayer = target;
        if (targetPlayer != null)
            targetPlayerController = targetPlayer.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("jumpInDuration", 1.0f / jumpInDuration);
        anim.SetFloat("jumpOutDuration", 1.0f / jumpOutDuration);
        switch (currentState)
        {
            case State.Stunned:
                // Debug.Log("State.Stunned");
                rb.linearVelocity = Vector2.zero;
                break;

            case State.ChaseTarget:
                // Debug.Log("State.ChaseTarget");
                if (targetPlayer == null) break;

                bool isSameLevel = IsPlayerOnSameLevel(targetPlayer);
                // If target player is no longer on the same platform, find a new target
                if (!isSameLevel && targetPlayerController.IsGrounded())
                    currentState = State.JumpIn;

                MoveTowards(targetPlayer);
                break;

            // Finds the target player to chase, teleports to them if necessary, stands still if no target found
            case State.FindTarget:
                // Debug.Log("State.FindTarget currentTarget" + targetPlayer);
                SetTarget(FindClosestPlayerOnSamePlatform());
                if (targetPlayer == null)
                    SetTarget(FindRandomGroundedPlayer(groundedPlayers));
                if (targetPlayer == null) break;
                // Debug.Log("Pursuing " + targetPlayer.name);
                if (!IsPlayerOnSamePlatform(targetPlayer))
                    if (TeleportToPlayerPlatform()) currentState = State.JumpOut;
                break;
            case State.JumpIn:
                Debug.Log("State.JumpIn");
                currentState = State.Stunned;
                StartCoroutine(JumpIn(jumpInDuration));
                break;

            case State.JumpOut:
                Debug.Log("State.JumpOut");
                currentState = State.Stunned;
                Debug.Log("State.JumpOutPre");
                StartCoroutine(JumpOut(jumpOutDuration));
                Debug.Log("State.JumpOutPost");
                break;
        }
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    IEnumerator JumpIn(float duration)
    {
        anim.ResetTrigger("JumpOutEnd");
        anim.SetTrigger("JumpIn");
        yield return new WaitForSeconds(duration);
        anim.ResetTrigger("JumpIn");
        currentState = State.FindTarget;
    }

    IEnumerator JumpOut(float duration)
    {
        anim.SetTrigger("JumpOut");
        yield return new WaitForSeconds(duration);
        anim.ResetTrigger("JumpOut");
        currentState = State.ChaseTarget;
        anim.SetTrigger("JumpOutEnd");
    }


    private void UpdateFacing()
    {
        float moveValue = rb.linearVelocity.x;
        if (moveValue > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (moveValue < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }

    bool HasGroundAhead(float direction)
    {
        float checkDistance = 0.25f;
        Vector2 origin = new Vector2(
            transform.position.x + (direction * checkDistance),
            transform.position.y
        );
        float rayDistance = bc.bounds.size.y * 0.6f + 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, platformMask);
        Debug.DrawRay(origin, Vector2.down * rayDistance, hit.collider ? Color.green : Color.red);
        if (!hit.collider) return false;
        return true;
    }

    void MoveTowards(GameObject target)
    {
        if (!IsPlayerOnSameLevel(target))
        {
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
            return;
        }

        float xDeltaFromTarget = target.transform.position.x - transform.position.x;
        float dir = Mathf.Sign(xDeltaFromTarget);

        if (!targetPlayerController.IsGrounded() && Mathf.Abs(xDeltaFromTarget) < 0.1)
        {
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
        }
        else if (HasGroundAhead(dir))
        {
            rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
        }
        else
        {
            // No ground ahead, stop moving
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
        }
        
        UpdateFacing();
    }

    GameObject FindClosestPlayerOnSamePlatform()
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        groundedPlayers = new List<GameObject>();

        foreach (var p in players)
        {
            // Skip inactive and jumping players
            if (!p.gameObject.activeInHierarchy || !p.GetComponent<PlayerController>().IsGrounded()) continue;

            groundedPlayers.Add(p);

            if (IsPlayerOnSamePlatform(p))
            {
                float dist = Vector2.Distance(transform.position, p.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = p;
                }
            }
        }

        return closest;
    }

    GameObject FindRandomGroundedPlayer(List<GameObject> groundedPlayers)
    {
        GameObject randomPlayer = null;

        if (groundedPlayers.Count > 0)
        {
            // pick a random grounded active player
            int randIndex = Random.Range(0, groundedPlayers.Count);
            randomPlayer = groundedPlayers[randIndex];

        }

        return randomPlayer;
    }

    bool IsPlayerOnSameLevel(GameObject player)
    {
        float inLevelThreshold = 0.25f;

        float playerFeetY = player.transform.position.y - player.transform.localScale.y;
        float myFeetY = transform.position.y - transform.localScale.y;

        return Mathf.Abs(playerFeetY - myFeetY) < inLevelThreshold;
    }

    bool IsPlayerOnSamePlatform(GameObject player)
    {
        if (!IsPlayerOnSameLevel(player)) return false;
        Vector2 playerPos = targetPlayer.transform.position;
        Vector2 targetBoundsCenter = targetPlayer.GetComponent<BoxCollider2D>().bounds.center;
        float dir = Mathf.Sign(playerPos.x - targetBoundsCenter.x);
        int i = 0;
        while (Mathf.Abs(i * stepSize * dir - playerPos.x) > 0.0f)
        {
            Vector2 checkPos = new Vector2(playerPos.x + i * stepSize * dir, playerPos.y);
            RaycastHit2D preHit = Physics2D.Raycast(checkPos, Vector2.right * dir, stepSize, platformMask);
            if (preHit.collider) return false;
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, stepSize, platformMask);
            if (!hit.collider) return false;
            i++;
        }
        return true;
    }
    
    bool TeleportToPlayerPlatform()
    {
        Vector2 playerPos = targetPlayer.transform.position;
        float targetY = playerPos.y - 0.5f * targetPlayer.GetComponent<BoxCollider2D>().bounds.size.y;

        List<float> xes = new();

        float left = playerPos.x - minDistanceToTeleportFromPlayer;
        for (int i = 0; i < 100; i++)
        {
            Vector2 checkPos = new Vector2(left - i * stepSize, playerPos.y);
            RaycastHit2D preHitLeft = Physics2D.Raycast(playerPos, Vector2.left, i * stepSize + minDistanceToTeleportFromPlayer, platformMask);
            if (preHitLeft.collider) break;
            // Debug.DrawRay(playerPos, Vector2.left * (i * stepSize + minDistanceToTeleportFromPlayer), Color.cyan, 2f);
            RaycastHit2D hitLeft = Physics2D.Raycast(checkPos, Vector2.down, stepSize, platformMask);
            if (hitLeft.collider && Mathf.Abs(hitLeft.point.y - targetY) < 0.1f)
                xes.Add(checkPos.x);
            else break;
            Debug.DrawRay(checkPos, Vector2.down * stepSize, Color.blue, 2f);
        }
        float right = playerPos.x + minDistanceToTeleportFromPlayer;
        for (int i = 0; i < 100; i++)
        {
            Vector2 checkPos = new Vector2(right + i * stepSize, playerPos.y);
            RaycastHit2D preHitRight = Physics2D.Raycast(playerPos, Vector2.right, i * stepSize + minDistanceToTeleportFromPlayer, platformMask);
            if (preHitRight.collider) break;
            // Debug.DrawRay(playerPos, Vector2.right * (i * stepSize + minDistanceToTeleportFromPlayer), Color.yellow, 2f);
            RaycastHit2D hitRight = Physics2D.Raycast(checkPos, Vector2.down, stepSize, platformMask);
            if (hitRight.collider && Mathf.Abs(hitRight.point.y - targetY) < 0.1f)
                xes.Add(checkPos.x);
            else break;
            Debug.DrawRay(checkPos, Vector2.down * stepSize, Color.blue, 2f);
        }

        if (xes.Count == 0)
        {
            // Debug.Log("cant teleport");
            currentState = State.FindTarget;
            // SetTarget(FindRandomGroundedPlayer(groundedPlayers));
            return false;
        }

        float randomX = xes[Random.Range(0, xes.Count)];

        float y = targetY + bc.bounds.size.y * 0.5f;

        // Debug.Log("teleporting");
        transform.position = new Vector3(randomX, y, transform.position.z);

        return true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(10);
        }
    }

}