using UnityEngine;
using UnityEngine.InputSystem;

public class MageAbilities : MonoBehaviour
{
    private FireballSpellController fireballSpell;

    [SerializeField] private SplitSpellController splitSpellController;

    private InputAction splitAction;
    private InputAction fireballAction;
    private InputAction aimAction;

    void Awake()
    {
        fireballSpell = GetComponent<FireballSpellController>();

        var input = GetComponent<PlayerInput>();
        if (input != null)
        {
            splitAction = input.actions["SplitSpell"];
            fireballAction = input.actions["FireballSpell"];
            aimAction = input.actions["SpellMovePrimary"];
        }
    }

    void Update()
    {

        if (splitAction != null && splitAction.triggered)
            splitSpellController.Cast();


        if (fireballAction != null && fireballAction.triggered)
        {
            Vector2 aim = aimAction.ReadValue<Vector2>();
            if (aim == Vector2.zero)
                aim = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            fireballSpell.Cast(aim);
        }

    }

    
}
