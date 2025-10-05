using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mage : PlayerMovement
{
    
    private InputAction splitSlimesInput;
    private InputAction lightSpellInput;
    private InputAction spellMoveInput;

    [SerializeField]  private SplitSpell splitSpell;
    [SerializeField] private GameObject lightSpellPrefab;

    private GameObject lightSpell;

    private bool enableMovement = true;
    protected override void Awake()
    {
        base.Awake();
        speed *= 0.3f;
        jumpSpeed *= 0.2f;
        wallJumpSpeed = 0;

        splitSlimesInput = playerInput.actions["Split"];
        lightSpellInput = playerInput.actions["Light"];
        spellMoveInput = playerInput.actions["SpellMove"];
    }

    protected override void HandleInput()
    {
        if (enableMovement)
        base.HandleInput();

        if (splitSlimesInput.triggered)
        {
            splitSpell.Cast();
        }
        if (lightSpellInput.triggered)
        {
            if (lightSpell == null)
            {
                lightSpell = Instantiate(lightSpellPrefab, transform.position, Quaternion.identity);
                lightSpell.GetComponent<LightSpell>().Cast();
            }
            else
            {
                lightSpell.GetComponent<LightSpell>().EndSpell();
                lightSpell = null;
            }
        }
            

    }

    public override void Jump()
    {
        if (enableMovement && grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }

    public InputAction GetLightSpellInput() { return lightSpellInput; }
    public InputAction GetSpellMoveInput() { return spellMoveInput; }

    public void SetControlEnabled(bool enabled) { enableMovement = enabled; }
}
