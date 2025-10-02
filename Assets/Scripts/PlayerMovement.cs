using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float wallJumpSpeed;

    private Rigidbody2D body;
    private bool grounded = false;
    private bool onWall = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (grounded)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
            }
            else if (onWall)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, wallJumpSpeed);
                onWall = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && collision.relativeVelocity.y >= 0)
            grounded = true;
        if (collision.gameObject.CompareTag("Wall"))
            onWall = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
        if (collision.gameObject.CompareTag("Wall"))
            onWall = false;
    }
}
