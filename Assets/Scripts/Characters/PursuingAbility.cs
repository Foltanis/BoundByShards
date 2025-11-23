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
                if (targetPlayer == null) break;
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
        float xDeltaFromTarget = target.transform.position.x - transform.position.x;
        float dir = Mathf.Sign(xDeltaFromTarget);

        if (!targetPlayerController.IsGrounded() && Mathf.Abs(xDeltaFromTarget) < 0.3)
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
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

        if (closest != null)
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

    bool TryFindTopEdgePolygon(CompositeCollider2D comp, Vector3 playerPos, out float left, out float right, out float surfaceY)
    {
        left = right = surfaceY = 0f;
        float bestDist = Mathf.Infinity;
        bool found = false;

        // Allocate a temp buffer. You can also allocate dynamically, but this is fine.
        Vector2[] points = new Vector2[32];

        for (int p = 0; p < comp.pathCount; p++)
        {
            int count = comp.GetPath(p, points);

            if (count < 2)
                continue;

            // Closed polygon: last point connects back to first
            for (int i = 0; i < count; i++)
            {
                Vector2 a = points[i];
                Vector2 b = points[(i + 1) % count]; // wrap-around

                bool horizontal = Mathf.Abs(a.y - b.y) < 0.01f;
                if (!horizontal) continue;

                float y = a.y;

                // Only consider edges BELOW the player
                if (playerPos.y < y) continue;

                float segLeft = Mathf.Min(a.x, b.x);
                float segRight = Mathf.Max(a.x, b.x);

                // Check if player's X is over that segment
                if (playerPos.x < segLeft - 0.01f || playerPos.x > segRight + 0.01f)
                    continue;

                // Distance to this surface
                float dist = Mathf.Abs(playerPos.y - y);

                if (dist < bestDist)
                {
                    bestDist = dist;
                    left = segLeft;
                    right = segRight;
                    surfaceY = y;
                    found = true;
                }
            }
        }

        return found;
    }

    float FindSafeTeleportY(CompositeCollider2D comp, Vector2 startPos)
    {
        float y = startPos.y;

        // Move up in tiny steps until you're clearly outside platform collider
        for (int i = 0; i < 20; i++)
        {
            if (!comp.OverlapPoint(new Vector2(startPos.x, y)))
                return y;

            y += 0.05f;  // climb in small increments
        }

        // fallback (just in case)
        return startPos.y + 1f;
    }

    void TeleportToPlayerPlatform()
    {
        GameObject platform = FindPlatformUndePlayer(targetPlayer);
        if (platform == null) return;

        CompositeCollider2D comp = platform.GetComponent<CompositeCollider2D>();
        if (comp == null)
        {
            Debug.LogWarning("Platform has no CompositeCollider2D.");
            return;
        }

        if (!TryFindTopEdgePolygon(comp, targetPlayer.transform.position, out float left, out float right, out float surfaceY))
        {
            Debug.LogWarning("Could not find top polygon edge.");
            return;
        }

        float playerX = targetPlayer.transform.position.x;

        float forbiddenLeft = playerX - minDistanceToTeleportFromPlayer;
        float forbiddenRight = playerX + minDistanceToTeleportFromPlayer;

        List<(float, float)> intervals = new();

        if (forbiddenLeft > left)
            intervals.Add((left, Mathf.Min(forbiddenLeft, right)));

        if (forbiddenRight < right)
            intervals.Add((Mathf.Max(forbiddenRight, left), right));

        if (intervals.Count == 0)
        {
            Debug.Log("No valid teleport interval.");
            return;
        }

        var interval = intervals[Random.Range(0, intervals.Count)];
        float randomX = Random.Range(interval.Item1, interval.Item2);

        float initialY = surfaceY + 0.1f;
        float y = FindSafeTeleportY(comp, new Vector2(randomX, initialY));

        transform.position = new Vector3(randomX, y, transform.position.z);
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