using UnityEngine;
using UnityEngine.InputSystem;

public class MageAbilities : MonoBehaviour
{
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private GameObject fireballPrefab;

    private InputAction splitAction;
    private InputAction lightAction;
    private InputAction fireballAction;
    private InputAction primarySpellMove;

    void Awake()
    {
        var input = GetComponent<PlayerInput>();
        if (input != null)
        {
            splitAction = input.actions["SplitSpell"];
            lightAction = input.actions["LightSpell"];
            fireballAction = input.actions["FireballSpell"];
            primarySpellMove = input.actions["SpellMovePrimary"];
        }
    }

    void Update()
    {
        SplitSpell.Instance.Update(Time.deltaTime);
        FireballSpell.Instance.Update(Time.deltaTime);

        if (splitAction != null && splitAction.triggered)
            SplitSpell.Instance.Cast();

        if (lightAction != null && lightAction.triggered)
            LightSpell.Instance.Cast(lightPrefab);

        if (fireballAction != null && fireballAction.triggered)
            FireballSpell.Instance.Cast(fireballPrefab, primarySpellMove.ReadValue<Vector2>());

    }

    
}
