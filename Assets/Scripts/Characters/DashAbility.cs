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

    private bool canDash = true;
    private Vector2 dashDirection;

    private float defaultGravityScale;
    private Coroutine dashRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.actions["Dash"];
        defaultGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (canDash && dashAction.triggered)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        //if (dashRoutine != null) StopCoroutine(dashRoutine);
        dashRoutine = StartCoroutine(DashRoutine());
        SoundManager.PlaySound(SoundType.SLIME_DASH, gameObject, 1);
    }

    private IEnumerator DashRoutine()
    {
        Debug.Log("Dash started");

        // Disable input
        playerController.enabled = false;

        // Determine dash direction
        dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Modify physics
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.linearVelocity = Vector2.zero;

        // Apply dash force
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

        canDash = false;

        uiCooldowns.StartCooldown(UICooldowns.AbilityType.Dash, dashCooldown);

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // End dash
        EndDash();

        // Start cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void EndDash()
    {
        rb.gravityScale = defaultGravityScale;
        rb.linearVelocity = Vector2.zero;

        playerController.ResetInput();
        playerController.enabled = true;

        Debug.Log("Dash ended");
    }
}
