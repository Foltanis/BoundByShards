using UnityEngine;

public class FireballSpellController : MonoBehaviour
{
    [SerializeField] private FireballData spellData;

    private float cooldownRemaining = 0f;

    private void Update()
    {
        if (cooldownRemaining > 0)
            cooldownRemaining -= Time.deltaTime;
    }

    public bool Cast(Vector2 direction)
    {
        if (cooldownRemaining > 0)
            return false;

        GameObject caster = gameObject;

        GameObject fb = Instantiate(spellData.fireballPrefab,
                                    caster.transform.position + Vector3.up * 0.5f,
                                    Quaternion.identity);

        fb.GetComponent<FireballController>()
            .Init(direction, spellData.projectileSpeed, spellData.damage, caster);

        cooldownRemaining = spellData.cooldown;
        return true;
    }

    public bool IsOnCooldown => cooldownRemaining > 0;
}
