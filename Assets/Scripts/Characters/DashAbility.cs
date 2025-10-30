using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class DashAbility : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 1000000f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;  

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    //private InputAction moveAction;
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
        playerInput = GetComponent<PlayerInput>();

        //moveAction = playerInput.actions["Move"];
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

        //Ak je pr·ve dash aktÌvny
        if (isDashing)
        {
            dashTimer.Update(Time.deltaTime);
            if (dashTimer.IsFinished())
                EndDash();
        }

        // Dash input
        if (canDash && dashAction.triggered /*&& !isDashing*/)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        Debug.Log("Dash started");
        //dashDirection = moveAction.ReadValue<float>();

        // ak sa nehybe, pouûijeme smer podæa otoËenia
        //if (Mathf.Abs(dashDirection) < 0.1f)
        //dashDirection = Mathf.Sign(transform.localScale.x);
        dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        rb.gravityScale = 0f;
        rb.linearDamping = 0f;

        //rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

        isDashing = true;
        canDash = false;

        cooldownTimer.Reset();
        dashTimer.Reset();
    }

    private void EndDash()
    {
        isDashing = false;
        canDash = true;

        rb.gravityScale = 1f; // sp‰ù na pÙvodn˙ hodnotu

        // Spomalenie po dashe(alebo nech·ö podæa fyziky)
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Dash ended");
        // e.g. animator.ResetTrigger("Dash");
    }

    //public bool IsDashing()
    //{
    //    return isDashing;
    //}
}

