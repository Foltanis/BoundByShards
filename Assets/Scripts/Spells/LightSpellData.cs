using UnityEngine;

[CreateAssetMenu(menuName = "Spells/LightSpell", fileName = "LightSpellData")]
public class LightSpellData : ScriptableObject
{
    [Header("Light Prefab")]
    public GameObject lightPrefab;

    [Header("Distance of lights to mage")]
    public float spawnDistance = 1.2f;
}
