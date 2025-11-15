using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class FireballData : ScriptableObject
{
    public GameObject fireballPrefab;
    public float cooldown = 1.5f;
    public float projectileSpeed = 5f;
    public int damage = 10;
    public Vector3 spawnOffset = new Vector3(0, 0.5f, 0);
}
