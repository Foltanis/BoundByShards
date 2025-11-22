using NUnit.Framework;  
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class PursuingAbility : MonoBehaviour
{
    [SerializeField] private LayerMask platformMask;

    private List<GameObject> players;
    private GameObject targetPlayer;
    private Rigidbody2D rb;
    private State currentState;
    private Vector3 baseScale;
    private PlayerController targetPlayerController;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float minDistanceToTeleportFromPlayer = 2.0f;
    private enum State
    {
        Stunned,
        ChaseTarget,
        FindTarget,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        players = new List<GameObject>();

        foreach (var character in CharacterManager.Instance.Characters)
        {

            if (character.instance != null)
                players.Add(character.instance);
        }

        currentState = State.FindTarget;

        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Stunned:
                rb.linearVelocity = Vector2.zero;
                break;

            case State.ChaseTarget:
                MoveTowards(targetPlayer);

                // If target player is no longer on the same platform, find a new target
                if (!IsPlayerOnSameLevel(targetPlayer) && targetPlayerController.IsGrounded())
                    currentState = State.FindTarget;
                break;

            // Finds the target player to chase, teleports to them if necessary, stands still if no target found
            case State.FindTarget:
                targetPlayer = FindClosestPlayerOnSameLevel();
                if (targetPlayer != null)
                {
                    currentState = State.ChaseTarget;

                    if (!IsPlayerOnSameLevel(targetPlayer))
                        TeleportToPlayerPlatform();

                }
                break;
        }

    }

    private void UpdateFacing()
    {
        float moveValue = rb.linearVelocity.x;
        if (moveValue > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (moveValue < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }

    void MoveTowards(GameObject target)
    {
        float magicConstant = 10f;
        float xDeltaFromTarget = target.transform.position.x - transform.position.x;
        float dir = Mathf.Sign(xDeltaFromTarget);

        // 
        if (!targetPlayerController.IsGrounded() && Mathf.Abs(xDeltaFromTarget) < 0.3/*speed * Time.fixedDeltaTime * magicConstant*/)
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
        //return
        else 
            rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
        
        UpdateFacing();
    }


    GameObject FindClosestPlayerOnSameLevel()
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        List<GameObject> groundedPlayers = new List<GameObject>();

        foreach (var p in players)
        {
            // Skip inactive and jumping players
            if (!p.gameObject.activeInHierarchy ||
                !p.GetComponent<PlayerController>().IsGrounded()) continue;

            groundedPlayers.Add(p);

            if (IsPlayerOnSameLevel(p))
            {
                float dist = Vector2.Distance(transform.position, p.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = p;
                }
            }
        }

        if (closest == null)
            closest = FindRandomGroundedPlayer(groundedPlayers);

        targetPlayerController = closest.GetComponent<PlayerController>();

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
        float inLevelThreshold = 0.15f;

        float playerFeetY = player.transform.position.y - player.transform.localScale.y;
        float myFeetY = transform.position.y - transform.localScale.y;

        return Mathf.Abs(playerFeetY - myFeetY) < inLevelThreshold;
    }

    void TeleportToPlayerPlatform()
    {
        // GameObject platformToTeleport = FindPlatformUndePlayer(targetPlayer);
        // if (platformToTeleport == null)
        //     return;

        // BoxCollider2D col = platformToTeleport.GetComponent<BoxCollider2D>();

        // // platform bounds
        // Bounds b = col.bounds;
        // float left = b.min.x;
        // float right = b.max.x;

        // float playerX = targetPlayer.transform.position.x;

        // // forbidden area around player
        // float forbiddenLeft = playerX - minDistanceToTeleportFromPlayer;
        // float forbiddenRight = playerX + minDistanceToTeleportFromPlayer;
        
        // List<(float, float)> intervals = new List<(float, float)>();

        // // intervals where teleport is allowed
        // if (forbiddenLeft > left)
        //     intervals.Add((left, forbiddenLeft));
        // // intervals.Add((left, Mathf.Min(forbiddenLeft, right))); why not this?

        // if (forbiddenRight < right)
        //     intervals.Add((forbiddenRight, right));

        // if (intervals.Count == 0)
        // {
        //     Debug.Log("No room to teleport on platform");
        //     return;
        // }

        
        // var randomInterval = intervals[Random.Range(0, intervals.Count)];
        // float randomX = Random.Range(randomInterval.Item1, randomInterval.Item2);

        
        // float y = b.max.y + transform.localScale.y + 0.01f; // slightly above platform

        // // teleport
        // transform.position = new Vector3(randomX, y, transform.position.z);
        // Debug.Log("Teleported to: " + transform.position);
    }

    GameObject FindPlatformUndePlayer(GameObject player)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            player.transform.position,
            Vector2.down,
            3f,
            platformMask
        );

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        Debug.LogWarning("Cant finde PLATFORM!!!!!!");
        return null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        //}
    }

}