using UnityEngine;
using UnityEngine.InputSystem;

public class MageAbilities : MonoBehaviour
{
    [SerializeField] private GameObject lightPrefab;

    private InputAction splitAction;
    private InputAction lightAction;

    void Awake()
    {
        var input = GetComponent<PlayerInput>();
        if (input != null)
        {
            splitAction = input.actions["SplitSpell"];
            lightAction = input.actions["LightSpell"];
        }
    }

    void Update()
    {
        SplitSpell.Instance.Update(Time.deltaTime);

        if (splitAction != null && splitAction.triggered)
            SplitSpell.Instance.Cast();

        if (lightAction != null && lightAction.triggered)
            LightSpell.Instance.Cast(lightPrefab);

    }

    
}
