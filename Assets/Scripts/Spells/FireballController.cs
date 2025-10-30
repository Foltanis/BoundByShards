using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballController : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Collider2D myCollider;
    Vector2 direction;
    private Collider2D casterCol;


    public void Init(Vector2 dir, GameObject caster)
    {
        direction = dir.normalized;
        casterCol = caster.GetComponent<Collider2D>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        rb.linearVelocity = direction * speed;

        if (casterCol != null)
        {
            Physics2D.IgnoreCollision(myCollider, casterCol, true);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.CompareTag("Enemy"))
        //{
        //    var hp = col.gameObject.GetComponent<Health>();
        //    if (hp != null)
        //        hp.TakeDamage(damage);
        //}

        Destroy(gameObject);
    }
}
