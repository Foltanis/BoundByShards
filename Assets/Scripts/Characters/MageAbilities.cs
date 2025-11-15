using UnityEngine;
using UnityEngine.InputSystem;

public class MageAbilities : MonoBehaviour
{
    //[SerializeField] private GameObject lightPrefab;
    //[SerializeField] private GameObject fireballPrefab;
    private FireballSpellController fireballSpell;

    [SerializeField] private SplitSpellController splitSpellController;

    private InputAction splitAction;
    //private InputAction lightAction;
    private InputAction fireballAction;
    private InputAction aimAction;

    void Awake()
    {
        fireballSpell = GetComponent<FireballSpellController>();

        var input = GetComponent<PlayerInput>();
        if (input != null)
        {
            splitAction = input.actions["SplitSpell"];
            //lightAction = input.actions["LightSpell"];
            fireballAction = input.actions["FireballSpell"];
            aimAction = input.actions["SpellMovePrimary"];
        }
    }

    void Update()
    {
        //FireballSpell.Instance.Update(Time.deltaTime);

        if (splitAction != null && splitAction.triggered)
            splitSpellController.Cast();

        //if (lightAction != null && lightAction.triggered)
        //    LightSpell.Instance.Cast(lightPrefab);

        //if (fireballAction != null && fireballAction.triggered)
        //    FireballSpell.Instance.Cast(fireballPrefab, primarySpellMove.ReadValue<Vector2>());

        if (fireballAction != null && fireballAction.triggered)
        {
            Vector2 aim = aimAction.ReadValue<Vector2>();
            if (aim == Vector2.zero)
                aim = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            fireballSpell.Cast(aim);
        }

    }

    
}
