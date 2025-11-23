using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballController : MonoBehaviour, IFreezableReceiver
{
    private Rigidbody2D rb;
    private Collider2D myCollider;

    private Vector2 direction;
    private float speed;
    private int damage;
    private Collider2D casterCollider;

    public void Init(Vector2 dir, float speed, int dmg, GameObject caster)
    {
        direction = dir;
        this.speed = speed;
        damage = dmg;
        casterCollider = caster.GetComponent<Collider2D>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        rb.linearVelocity = direction * speed;
        Physics2D.IgnoreCollision(myCollider, casterCollider, true);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var hp = col.gameObject.GetComponent<Health>();
        if (hp != null)
            hp.TakeDamage(damage);

        Destroy(gameObject);
    }

    public void CastOnFreeze()
    {
        Destroy(gameObject);
    }

    public void CastOnUnfreeze()
    {
        // No action needed on unfreeze for the fireball
    }
}
