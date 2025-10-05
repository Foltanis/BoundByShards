using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSpell : Spells
{
    [SerializeField] private GameObject lightPrefab;
    private GameObject light1;
    private GameObject light2;

    private InputAction Light1Move;
    private InputAction Light2Move;

    public override void Awake()
    {
        base.Awake();
        
        Light2Move = mage.GetSpellMoveInput();
        Light1Move = mage.GetMoveAction();
    }

    public override void Cast()
    {

        // zablokujeme pohyb mága
        mage.SetControlEnabled(false);

        // pozícia maga
        Vector3 pos = mage.transform.position;

        // vytvoríme 2 svetlá
        light1 = Instantiate(lightPrefab, pos + new Vector3(-1, 0, 0), Quaternion.identity);
        light2 = Instantiate(lightPrefab, pos + new Vector3(1, 0, 0), Quaternion.identity);

        // napr. pridáme skripty na ovládanie svetiel
        light1.AddComponent<LightController>().Init(Light1Move);
        light2.AddComponent<LightController>().Init(Light2Move);
    }

    public void EndSpell()
    {
        mage.SetControlEnabled(true);

        if (light1) Destroy(light1);
        if (light2) Destroy(light2);

        
    }
}