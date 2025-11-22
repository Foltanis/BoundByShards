using UnityEngine;

public class GolemPatrol : MonoBehaviour
{
    [SerializeField] private float speed = 2f;        // movement speed
    [SerializeField] private float period = 2f;       // time it takes to move in one direction
    [SerializeField] private bool startMovingRight = true;

    private int direction;
    private float timer;

    private void Start()
    {
        direction = startMovingRight ? 1 : -1;
        timer = period * 0.5f;
    }

    private void Update()
    {
        // Move based on direction
        transform.Translate(speed * direction * Time.deltaTime, 0f, 0f);

        // Count up time
        timer += Time.deltaTime;

        // When period finishes, switch direction
        if (timer >= period)
        {
            timer = 0f;
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        direction = -direction; // flip direction

        // Flip sprite visually
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}
