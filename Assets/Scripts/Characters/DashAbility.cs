using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class DashAbility : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 200f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;

    private Rigidbody2D rb;
    private PlayerController playerController;
    private PlayerInput playerInput;
    private InputAction dashAction;

    private bool isDashing = false;
    private bool canDash = true;
    private Vector2 dashDirection;
    private Timer dashTimer;
    private Timer cooldownTimer;

    private float defaultGravityScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dash"];
        defaultGravityScale = rb.gravityScale;

        dashTimer = new Timer(dashDuration);
        cooldownTimer = new Timer(dashCooldown);
    }

    void Update()
    {
        // Cooldown management
        if (!canDash)
        {
            cooldownTimer.Update(Time.deltaTime);
            if (cooldownTimer.IsFinished())
                canDash = true;
        }

        // Dash management
        if (isDashing)
        {
            dashTimer.Update(Time.deltaTime);
            if (dashTimer.IsFinished())
                EndDash();
        }

        // Dash input
        if (canDash && dashAction.triggered)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        Debug.Log("Dash started");

        playerController.enabled = false;

        dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

        isDashing = true;
        canDash = false;

        dashTimer.Reset();
        cooldownTimer.Reset();
    }

    private void EndDash()
    {
        isDashing = false;
        rb.gravityScale = defaultGravityScale;
        rb.linearVelocity = Vector2.zero;

        playerController.ResetInput();
        playerController.enabled = true;

        Debug.Log("Dash ended");
    }
}
