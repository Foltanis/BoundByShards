using UnityEngine;

[RequireComponent(typeof(FireballSpellController))]
public class OccultistAbility : MonoBehaviour
{

    private FireballSpellController fireballSpell;
    private Vector2 facingDiraction;

    private void Awake()
    {
        fireballSpell = GetComponent<FireballSpellController>();
        facingDiraction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    private void Update()
    {
        
        fireballSpell.Cast(facingDiraction);
    }
}
