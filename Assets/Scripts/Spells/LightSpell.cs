using UnityEngine;
using UnityEngine.InputSystem;

public class LightSpell
{
    private static LightSpell _instance;
    public static LightSpell Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LightSpell();
            return _instance;
        }
    }

    private GameObject mage;
    private PlayerInput mageInput;

    private GameObject light1;
    private GameObject light2;

    private InputAction movePrimary;
    private InputAction moveSecondary;
    private InputAction endSpell;

    private bool active = false;

    
    private LightSpell()
    {
        var cm = CharacterManager.Instance;

        
        mage = cm.GetPrefab("Mage");
        mageInput = mage.GetComponent<PlayerInput>();
        
        
        movePrimary = mageInput.actions["SpellMovePrimary"];
        moveSecondary = mageInput.actions["SpellMoveSecondary"];
        endSpell = mageInput.actions["LightSpell"];
    }

    public void Cast(GameObject lightPrefab)
    {
        if (active)
        {
            EndSpell();
            return;
        }
            
        active = true;

        mageInput.enabled = false;
        mageInput.actions["LightSpell"].Enable();

        Vector3 pos = mage.transform.position;
        light1 = Object.Instantiate(lightPrefab, pos + Vector3.left, Quaternion.identity);
        light2 = Object.Instantiate(lightPrefab, pos + Vector3.right, Quaternion.identity);

        light1.AddComponent<LightController>().Init(movePrimary);
        light2.AddComponent<LightController>().Init(moveSecondary);
    }

    public void EndSpell()
    {
        active = false;
        mageInput.enabled = true;

        if (light1) Object.Destroy(light1);
        if (light2) Object.Destroy(light2);
    }
}
