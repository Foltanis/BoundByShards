using UnityEngine;
using UnityEngine.Timeline;

public class GolemPatrol : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private bool startMovingRight = true;

    private Vector2 startPosition;
    private int direction;

    private void Start()
    {
        // set starting position, direction, facing right or left
        ChangeDirection(startMovingRight ? 1 : -1);
    }

    private void Update()
    {
        // movement in x direction
        float moveStep = speed * Time.deltaTime * direction;
        transform.Translate(moveStep, 0f, 0f);

        float currentOffset = Mathf.Abs(transform.position.x - startPosition.x);

        if (currentOffset >= moveDistance)
        {
            if (direction == 1)
                ChangeDirection(-1);
            else
                ChangeDirection(1);
        }
    }

    private void ChangeDirection(int newDirection)
    {
        startPosition = transform.position;
        direction = newDirection;

        // rotate the golem to face the new direction
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}