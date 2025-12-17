using UnityEngine;
using System.Collections;

public class GolemAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float killRadius = 1.2f;
    [SerializeField] private float shockwaveRadius = 3.5f;
    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Layers")]
    [SerializeField] private LayerMask playerMask;

    private Animator animator;

    private bool isAttacking = false;
    private float lastAttackTime;

    private PatrolAbility patrolAbility;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        patrolAbility = GetComponent<PatrolAbility>();
    }

    private void Update()
    {
        if (isAttacking) return;

        Collider2D player = Physics2D.OverlapCircle(
            transform.position,
            attackRange,
            playerMask
        );

        if (player != null && Time.time > lastAttackTime + attackCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        patrolAbility.enabled = false;

        animator.SetTrigger("attack");


        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        patrolAbility.enabled = true;
    }

    public void ApplyAttackEffects()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(
            transform.position,
            shockwaveRadius,
            playerMask
        );

        foreach (var col in players)
        {
            float dist = Vector2.Distance(transform.position, col.transform.position);

            if (dist <= killRadius)
            {
                col.GetComponent<Health>().TakeDamage(20);
            }
            else
            {
                PlayerController playerCon = col.GetComponent<PlayerController>();
                if (playerCon != null)
                {
                    Vector2 dir = (col.transform.position - transform.position).normalized;
                    playerCon.Knockback(dir * knockbackForce);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, killRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
