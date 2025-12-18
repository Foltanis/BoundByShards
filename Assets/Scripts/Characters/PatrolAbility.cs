using UnityEngine;

public class PatrolAbility : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private bool startMovingRight = true;

    [Header("Moonwalking fix")]
    [SerializeField] private bool defaultFacingRight = true;

    private Vector2 startPosition;
    private int direction;
    private int facingMultiplier;

    private void Start()
    {
        // sprite correction factor
        facingMultiplier = defaultFacingRight ? 1 : -1;

        startPosition = transform.position;
        ChangeDirection(startMovingRight ? 1 : -1);
    }

    private void Update()
    {
        float moveStep = speed * Time.deltaTime * direction;
        transform.Translate(moveStep, 0f, 0f, Space.World);

        float currentOffset = Mathf.Abs(transform.position.x - startPosition.x);

        if (currentOffset >= moveDistance)
        {
            ChangeDirection(-direction);
        }
    }

    private void ChangeDirection(int newDirection)
    {
        direction = newDirection;
        startPosition = transform.position;

        // sprite flip with orientation correction
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * direction * facingMultiplier,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}
