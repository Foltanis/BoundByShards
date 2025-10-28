using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpSpeed = 6f;
    [SerializeField] private float wallJumpSpeed = 5f;

    private Rigidbody2D body;
    private bool grounded;
    private bool onWall;
    private Vector3 baseScale;

    private InputAction moveAction;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;

        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            Debug.Log("asfjnkdjnfjkdasf");
            moveAction = playerInput.actions["Move"];
            Debug.Log(playerInput.currentActionMap?.name);
        }
    }

    void Update()
    {
        if (moveAction == null) return;

        Vector2 move = moveAction.ReadValue<Vector2>();
        body.linearVelocity = new Vector2(move.x * speed, body.linearVelocity.y);

        Debug.Log(move.x);

        if (move.x > 0.01f)
            Debug.Log("move");
        else if (move.x < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);

    }

    public void TryJump()
    {
        Debug.Log("jump");
        if (grounded)
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        else if (onWall)
            body.linearVelocity = new Vector2(body.linearVelocity.x, wallJumpSpeed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) grounded = true;
        if (col.gameObject.CompareTag("Wall")) onWall = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) grounded = false;
        if (col.gameObject.CompareTag("Wall")) onWall = false;
    }
}
