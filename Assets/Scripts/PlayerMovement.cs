using UnityEngine;

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

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleInput();
    }

    protected abstract void HandleInput();

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
