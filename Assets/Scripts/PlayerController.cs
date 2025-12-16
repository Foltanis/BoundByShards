using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour, IFreezableReceiver
{
    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float wallJumpForce = 4f;

    [Header("Detection")]
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float wallCheckDistance = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool showDebugGizmos = true;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    private float moveValue;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Vector3 baseScale;

    //private Health health;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool wallJumpLock;
    private int wallDirection;

    private bool frozen = false;
    // TODO: fix movement when frozen, mage is moving after freeze if was moving before
    public bool IsGrounded() => isGrounded;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.currentActionMap;
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");

        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        baseScale = transform.localScale;

        //health = GetComponent<Health>();
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
        CheckGrounded();
        CheckWall();
        HandleMovement();
        UpdateFacing();
        anim.SetBool("IsGrounded", isGrounded);
    }

    private void CheckGrounded()
    {
        Vector2 origin = boxCollider.bounds.center;
        Vector2 size = boxCollider.bounds.size;
        Vector2 boxCenter = origin + Vector2.down * (size.y + groundCheckDistance) * 0.5f;
        Vector2 boxSize = new Vector2(size.x * 0.9f, groundCheckDistance);
        Collider2D hit = Physics2D.OverlapBox(
            boxCenter,
            boxSize,
            0f,
            platformLayer
        );
        isGrounded = hit != null;
    }

    private void CheckWall()
    {
        Vector2 origin = boxCollider.bounds.center;
        Vector2 size = boxCollider.bounds.size;
        Vector2 boxSize = new Vector2(wallCheckDistance, size.y * 0.9f);
        Collider2D hitLeft = Physics2D.OverlapBox(
            origin + Vector2.left * (size.x + wallCheckDistance) * 0.5f,
            boxSize,
            0f,
            platformLayer
        );
        Collider2D hitRight = Physics2D.OverlapBox(
            origin + Vector2.right * (size.x + wallCheckDistance) * 0.5f,
            boxSize,
            0f,
            platformLayer
        );
        // Debug.Log("isGrounded " + isGrounded + "  hitLeft " + (hitLeft != null) + "  hitRight " + (hitRight != null));
        isTouchingWall = (hitLeft != null) || (hitRight != null);
    }

    private void HandleMovement()
    {
        if (frozen) return;
        body.linearVelocity = new Vector2(moveValue * speed, body.linearVelocity.y);
    }

    private void UpdateFacing()
    {
        if (moveValue > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (moveValue < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveValue = ctx.ReadValue<float>();
        anim.SetFloat("Speed", Mathf.Abs(moveValue) * Mathf.Sqrt(speed));
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveValue = 0;
        anim.SetFloat("Speed", 0);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        float jumpValue = ctx.ReadValue<float>();
        if (isGrounded)
        {
            Vector2 ba = Vector3.up * jumpValue * jumpForce;
            body.AddForce(ba, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
        else if (isTouchingWall)
        {
            float wallDir = transform.localScale.x > 0 ? -1 : 1;
            Vector2 force = new Vector2(wallDir * wallJumpForce, jumpForce);
            body.linearVelocity = force;
            anim.SetTrigger("Jump");
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        isTouchingWall = false; // to stop climbing the wall
    }

    public void ResetInput()
    {
        moveValue = 0f;
    }

    public void CastOnFreeze()
    {
        body.linearVelocity = Vector2.zero;
        enabled = false;
    }

    public void CastOnUnfreeze()
    {
        enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos || boxCollider == null)
            return;

        Vector2 origin = boxCollider.bounds.center;
        Vector2 size = boxCollider.bounds.size;

        // Ground check box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            origin + Vector2.down * (size.y + groundCheckDistance) * 0.5f,
            new Vector2(size.x * 0.9f, groundCheckDistance)
        );

        // Wall check boxes
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            origin + Vector2.right * (size.x + wallCheckDistance) * 0.5f,
            new Vector2(wallCheckDistance, size.y * 0.9f)
        );

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(
            origin + Vector2.left * (size.x + wallCheckDistance) * 0.5f,
            new Vector2(wallCheckDistance, size.y * 0.9f)
        );
    }
}