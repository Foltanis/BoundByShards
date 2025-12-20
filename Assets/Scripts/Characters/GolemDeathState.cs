using UnityEngine;

public class GolemDeathState : MonoBehaviour
{
    [SerializeField] Vector2 deathBodySize = new Vector2(1.5f, 1.0f);
    [SerializeField] Vector2 deathBodyOffset = new Vector2(0f, -0.5f);


    private PatrolAbility patrolAbility;
    private GolemAttack attack;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    
    private void Awake()
    {
        patrolAbility = GetComponent<PatrolAbility>();
        attack = GetComponent<GolemAttack>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        Debug.Log("UsedByComposite: " + col.compositeOperation);
    }

    // called from Golem Animator
    public void OnDeathAnimationComplete()
    {
        Destroy(patrolAbility);
        Destroy(attack);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        Vector3 scale = transform.localScale;

        col.size = new Vector2(
            deathBodySize.x,
            deathBodySize.y
        );

        col.offset = new Vector2(
            deathBodyOffset.x,
            deathBodyOffset.y
        );

        Debug.Log("Collider size after death: " + col.size);
    }
}
