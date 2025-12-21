using UnityEngine;
using System.Collections;

public class EyeAbility : MonoBehaviour
{
    [Header("Chase")]
    public float chaseSpeed = 3f;
    public float keepDistance = 2.5f;

    [Header("Vision")]
    public LayerMask wallMask;
    public float lostSightDelay = 3f;

    [Header("Rotation By Movement")]
    public float rotationSpeed = 10f;
    public float rotationOffset = 0f;

    private Transform player;
    private Transform eye;
    private Vector3 patrolPosition;
    private PatrolAbility patrolAbility;

    private bool playerVisible = false;
    private Coroutine lostSightCoroutine;
    private bool wasPlayerVisible = false;

    private bool frozen = false;

    void Start()
    {
        //maybe possition is enough TODO
        eye = transform.parent;
        patrolAbility = eye.GetComponent<PatrolAbility>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            //Debug.Log("Player entered eye trigger");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform == player)
        {
            player = null;
            //Debug.Log("Player exited eye trigger");
        }
    }
    void Update()
    {
        if (frozen) {
            return;
        }


        if (player == null)
        {
            if (wasPlayerVisible)
            {
                wasPlayerVisible = false;
                playerVisible = false;
                HandleLostPlayer();
            }
            return;
        }
        Vector2 dir = (player.position - eye.position);
        float dist = dir.magnitude;
        dir.Normalize();
        // Raycast to check for walls between eye and player
        RaycastHit2D hit = Physics2D.Raycast(eye.position, dir, dist, wallMask);

        if (hit.collider != null)
        {
            // Wall blocking view
            Debug.DrawLine(eye.position, hit.point, Color.red);

            if (wasPlayerVisible)
            {
                // Just lost sight
                wasPlayerVisible = false;
                playerVisible = false;
                HandleLostPlayer();
        }
        }
        else
        {
            // Clear line of sight
            Debug.DrawLine(eye.position, player.position, Color.green);

            if (!wasPlayerVisible)
            {
                // Just gained sight
                wasPlayerVisible = true;
            playerVisible = true;

                // Cancel return to patrol if running
                if (lostSightCoroutine != null)
                {
                    StopCoroutine(lostSightCoroutine);
                    lostSightCoroutine = null;
                }

                // Signal fireballs
                FireballSignalBroadcaster.EnemySeen(player.gameObject);

            if (patrolAbility != null)
                {
                    patrolAbility.enabled = false;
                    patrolPosition = eye.position;
                }

                //Debug.Log("Eye spotted player!");
            }
            else
            {
                // Continue signaling while player is visible
                FireballSignalBroadcaster.EnemySeen(player.gameObject);
        }

            ChasePlayer(dist, dir);
        }
    }
    void ChasePlayer(float dist, Vector2 dir)
    {
        if (dist > keepDistance)
        {
            eye.position = Vector2.MoveTowards(
                eye.position,
                player.position - (Vector3)(dir * keepDistance),
                chaseSpeed * Time.deltaTime
            );
        }
    }
    void HandleLostPlayer()
    {
        // Only start the coroutine if not already running
        if (lostSightCoroutine == null)
        {
            FireballSignalBroadcaster.EnemyLost();
            lostSightCoroutine = StartCoroutine(ReturnToPatrol());
            //Debug.Log("Eye lost player, returning to patrol");
        }
    }
    IEnumerator ReturnToPatrol()
    {
        yield return new WaitForSeconds(lostSightDelay);
        // go to default position
        while (Vector2.Distance(eye.position, patrolPosition) > 0.05f)
        {
            eye.position = Vector2.MoveTowards(
                eye.position,
                patrolPosition,
                chaseSpeed * Time.deltaTime
            );
            yield return null;
        }
        if (patrolAbility != null)
            patrolAbility.enabled = true;

        lostSightCoroutine = null;
    }

    public void CastOnFreeze()
    {
        frozen = true;
        // Stop all movement
        if (lostSightCoroutine != null)
        {
            StopCoroutine(lostSightCoroutine);
            lostSightCoroutine = null;
        }
        if (patrolAbility != null)
            patrolAbility.enabled = false;
        Debug.Log("Eye frozen");
    }

    public void CastOnUnfreeze()
    {
        frozen = false;
        // Resume patrol
        if (patrolAbility != null)
            patrolAbility.enabled = true;
    }

    //TODO eye on freeze behavior, problem is this is child of eye
    //public void CastOnFreeze()
    //{
    //    // Stop all movement
    //    if (lostSightCoroutine != null)
    //    {
    //        StopCoroutine(lostSightCoroutine);
    //        lostSightCoroutine = null;
    //    }
    //    if (patrolAbility != null)
    //        patrolAbility.enabled = false;
    //}

    //public void CastOnUnfreeze()
    //{
    //    // Resume patrol
    //    if (patrolAbility != null)
    //        patrolAbility.enabled = true;
    //}

}
