using UnityEngine;

public class EyeAbility : MonoBehaviour
{
    public float chaseSpeed = 3f;
    public LayerMask wallMask; // mask for walls that can block vision

    private Transform player;
    private Transform eye;
    private PatrolAbility patrolAbility;

    private bool playerVisible = false;
    void Start()
    {
        //maybe possition is enough TODO
        eye = transform.parent;
        patrolAbility = eye.GetComponent<PatrolAbility>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            player = other.transform;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            player = null;
    }

    void Update()
    {
        if (player == null)
            return;

        Vector2 dir = (player.position - eye.position).normalized;
        float dist = Vector2.Distance(player.position, eye.position);

        // RAYCAST pre overenie ?i nie je hrá? za stenou
        RaycastHit2D hit = Physics2D.Raycast(eye.position, dir, dist, wallMask);

        if (hit.collider != null)
        {
            playerVisible = false;
            Debug.DrawLine(eye.position, hit.point, Color.red);
            Debug.Log("Hrá? je v kuželi, ale skrytý za stenou");
        }
        else
        {
            playerVisible = true;
            if (patrolAbility != null)
                patrolAbility.enabled = false; // disable patrol when player is visible
            Debug.DrawLine(eye.position, player.position, Color.green);
            Debug.Log("Hrá? je VIDITE?NÝ");
        }

        if (playerVisible)
        {
            eye.position = Vector2.MoveTowards(
                eye.position,
                player.position,
                chaseSpeed * Time.deltaTime
            );
        }
    }
}
