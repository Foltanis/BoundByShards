using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MakesDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private bool affectOnlyPlayer = true;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            if (affectOnlyPlayer)
            {
                var player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    health.TakeDamage(damageAmount);
                }
            }
            else
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
