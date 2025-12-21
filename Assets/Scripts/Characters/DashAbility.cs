using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class DashAbility : MonoBehaviour
{
    [SerializeField] private UICooldowns uiCooldowns;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 200f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;

    private Rigidbody2D rb;
    private PlayerController playerController;
    private PlayerInput playerInput;
    private InputAction dashAction;

    private bool cooldown = false;
    private float timer = 0f;

    private Vector2 dashDirection;
    private float defaultGravityScale;

    private Coroutine dashRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dash"];

        defaultGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        if (cooldown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                cooldown = false;
        }

        if (dashAction.triggered)
            TryDash();
    }

    private void TryDash()
    {
        if (cooldown) return;

        dashRoutine = StartCoroutine(DashRoutine());

        cooldown = true;
        timer = dashCooldown;

        uiCooldowns.StartCooldown(
            UICooldowns.AbilityType.Dash,
            dashCooldown
        );

        SoundManager.PlaySound(SoundType.SLIME_DASH, gameObject, 1);
    }

    private IEnumerator DashRoutine()
    {
        // Disable input
        playerController.enabled = false;

        // Direction
        dashDirection = transform.localScale.x > 0
            ? Vector2.right
            : Vector2.left;

        // Physics setup
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.linearVelocity = Vector2.zero;

        // Dash impulse
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);

        EndDash();
    }

    private void EndDash()
    {
        rb.gravityScale = defaultGravityScale;
        rb.linearVelocity = Vector2.zero;

        playerController.ResetInput();
        playerController.enabled = true;
    }
}
