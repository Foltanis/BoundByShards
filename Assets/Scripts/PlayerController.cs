using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float wallJumpForce = 4f;

    private float moveValue;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Rigidbody2D body;
    private Health health;
    private bool grounded;
    private bool onWall;
    private Vector3 baseScale;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.currentActionMap;
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");

        body = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;

        Health health = GetComponent<Health>();
    }

    void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        jumpAction.performed += OnJumpPerformed;
        jumpAction.canceled += OnJumpCanceled;
    }

    void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        jumpAction.performed -= OnJumpPerformed;
        jumpAction.canceled -= OnJumpCanceled;
    }

    void FixedUpdate() 
    {
        body.linearVelocity = new Vector2(moveValue * speed, body.linearVelocity.y);

        // rotate character in direction of movement
        if (moveValue > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (moveValue < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveValue = ctx.ReadValue<float>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveValue = 0;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        float jumpValue = ctx.ReadValue<float>();
        if (grounded)
        {
            Vector2 ba = Vector3.up * jumpValue * jumpForce;
            body.AddForce(ba, ForceMode2D.Impulse);
            
        }
        else if (onWall)
        {
            float wallDir = transform.localScale.x > 0 ? -1 : 1;
            Vector2 force = new Vector2(wallDir * wallJumpForce, jumpForce);
            body.linearVelocity = force;
        }




    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        //onWall = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) grounded = true;
        if (col.gameObject.CompareTag("Wall")) onWall = true;
        if (col.gameObject.CompareTag("Enemy")) health.TakeDamage(1);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) grounded = false;
        if (col.gameObject.CompareTag("Wall")) onWall = false;
    }

    
}