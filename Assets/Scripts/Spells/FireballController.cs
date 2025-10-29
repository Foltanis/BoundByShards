using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballController : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    Vector2 direction;


    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = direction * speed;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.CompareTag("Enemy"))
        //{
        //    var hp = col.gameObject.GetComponent<Health>();
        //    if (hp != null)
        //        hp.TakeDamage(damage);
        //}

        //TODO: ignore collisions with the mage

        Destroy(gameObject);
    }
}
