using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSpellController : MonoBehaviour, IFreezableReceiver
{
    [SerializeField] private LightSpellData spellData; // ScriptableObject reference

    private PlayerInput input;

    private InputAction movePrimary;
    private InputAction moveSecondary;
    private InputAction toggleSpell;

    private LightController light1;
    private LightController light2;

    private bool isActive = false;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        movePrimary = input.actions["SpellMovePrimary"];
        moveSecondary = input.actions["SpellMoveSecondary"];
        toggleSpell = input.actions["LightSpell"];
    }

    private void Update()
    {

        if (toggleSpell != null && toggleSpell.triggered)
            ToggleSpell();
    }

    private void ToggleSpell()
    {
        if (!isActive) Cast();
        else EndSpell();
    }

    private void Cast()
    {
        isActive = true;

        // disable player movement inputs during spell
        input.enabled = false;
        input.actions["SplitSpell"].Enable();
        input.actions["LightSpell"].Enable();

        Vector3 center = transform.position;

        light1 = Instantiate(spellData.lightPrefab, center + Vector3.left * spellData.spawnDistance, Quaternion.identity)
            .AddComponent<LightController>();
        light2 = Instantiate(spellData.lightPrefab, center + Vector3.right * spellData.spawnDistance, Quaternion.identity)
            .AddComponent<LightController>();

        light1.Init(moveSecondary); 
        light2.Init(movePrimary);
    }

    private void EndSpell()
    {
        isActive = false;

        input.enabled = true;

        if (light1) Destroy(light1.gameObject);
        if (light2) Destroy(light2.gameObject);
    }

    public void CastOnFreeze()
    {
        enabled = false;
        if (light1 != null) light1.enabled = false;
        if (light2 != null) light2.enabled = false;
    }

    public void CastOnUnfreeze()
    {
        enabled = true;
        if (light1 != null) light1.enabled = true;
        if (light2 != null) light2.enabled = true;
    }
}