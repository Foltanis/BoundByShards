using UnityEngine;
using UnityEngine.InputSystem;

public class FireballSpell
{
    private static FireballSpell _instance;
    public static FireballSpell Instance
    {
        get
        {
            if (_instance == null)
                _instance = new FireballSpell();
            return _instance;
        }
    }

    private GameObject mage;

    private float cooldownTime = 1.5f; 
    private Timer cooldown;

    private bool cooldownOn = false;

    private FireballSpell()
    {
        mage = CharacterManager.Instance.GetPrefab("Mage");
        cooldown = new Timer(cooldownTime);
    }


    public void Update(float deltaTime)
    {
        if (cooldownOn)
        {
            cooldown.Update(deltaTime);
            if (cooldown.IsFinished())
                cooldownOn = false;
        }
    }

    public void Cast(GameObject fireballPrefab, Vector2 aimDir)
    {
        if (cooldownOn) return;

        Vector3 spawnPos = mage.transform.position + Vector3.up * 0.5f;

        // fallback – if no aim direction, shoot in facing direction
        if (aimDir == Vector2.zero)
            aimDir = mage.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        
        GameObject fb = Object.Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        fb.GetComponent<FireballController>().Init(aimDir);

        cooldownOn = true;
        cooldown.Reset();
    }
}
