using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected float speed = 6f;
    [SerializeField] protected float jumpSpeed = 6f;
    [SerializeField] protected float wallJumpSpeed = 5f;
    [SerializeField] protected int maxHealth = 6;

    protected Rigidbody2D body;
    protected bool grounded;
    protected bool onWall;

    protected Vector3 baseScale;

    protected int currentHealth;

    protected PlayerInput playerInput;
    protected InputAction moveAction;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;
        currentHealth = maxHealth;

        playerInput = GetComponent<PlayerInput>();

        
        moveAction = playerInput.actions["Move"];
    }

    protected virtual void Update()
    {
        HandleInput();
    }

    protected virtual void HandleInput()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        float horizontal = move.x;

        body.linearVelocity = new Vector2(horizontal * speed, body.linearVelocity.y);

        
        if (horizontal > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (horizontal < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);

        
        if (move.y > 0.5f)
            Jump();
    }

    public virtual void Jump()
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

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        gameObject.SetActive(false); 
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
        if (collision.gameObject.CompareTag("Wall"))
            onWall = true;

        if (collision.gameObject.CompareTag("Enemy"))
            TakeDamage(1);
        
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
        if (collision.gameObject.CompareTag("Wall"))
            onWall = false;
    }
}
