using UnityEngine;
using UnityEngine.InputSystem;

public class LightSpell : Spells
{
    [SerializeField] private GameObject lightPrefab;
    private GameObject light1;
    private GameObject light2;
    private bool active = false;

    private InputAction Light1Move;
    private InputAction Light2Move;

    public override void Awake()
    {
        //base.Awake();
        //// na��tame vstupy z maga
        //Light2Move = mage.GetSpellMoveInput();
        //Light1Move = mage.GetMoveAction();
    }

    private void Update()
    {
        //// sledujeme, �i bolo stla�en� X
        //if (lightToggle.triggered)
        //{
        //    if (active)
        //        EndSpell();
        //    else
        //        Cast();
        //}
        //
        //// pr�padne sem m��eme nesk�r prida� pohyb svetiel
    }

    public override void Cast()
    {
        //if (active) return; // ak u� be��, nerob ni�
        //active = true;
        //
        //// zablokujeme pohyb m�ga
        //mage.SetControlEnabled(false);
        //
        //// poz�cia maga
        //Vector3 pos = mage.transform.position;
        //
        //// vytvor�me 2 svetl�
        //light1 = Instantiate(lightPrefab, pos + new Vector3(-1, 0, 0), Quaternion.identity);
        //light2 = Instantiate(lightPrefab, pos + new Vector3(1, 0, 0), Quaternion.identity);
        //
        //// napr. prid�me skripty na ovl�danie svetiel
        //light1.AddComponent<LightController>().Init(LightController.ControlType.WASD);
        //light2.AddComponent<LightController>().Init(LightController.ControlType.Spell);
    }

    private void EndSpell()
    {
        //active = false;
        //mage.SetControlEnabled(true);

        //if (light1) Destroy(light1);
        //if (light2) Destroy(light2);
    }
}