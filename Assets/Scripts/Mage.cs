using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mage : PlayerMovement
{
    
    private InputAction splitSlimes;
    private InputAction lightSpell;
    private InputAction spellMove;

    [SerializeField]  private SplitSpell splitSpell;
    [SerializeField] private GameObject lightSpellPrefab;


    protected override void Awake()
    {
        base.Awake();
        speed *= 0.3f;
        jumpSpeed *= 0.2f;
        wallJumpSpeed = 0;

        splitSlimes = playerInput.actions["Split"];
        lightSpell = playerInput.actions["Light"];
        spellMove = playerInput.actions["SpellMove"];
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        if (splitSlimes.triggered)
        {
            splitSpell.Cast();
        }
        if (lightSpell.triggered)
            ;
    }

    public override void Jump()
    {
        if (grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }

    public InputAction GetLightSpellInput() { return lightSpell; }
    public InputAction GetSpellMoveInput() { return spellMove; }


}
