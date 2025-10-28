using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 6;
    private int currentHealth;

    void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log($"{name} took {dmg} damage ? HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{name} died!");
        gameObject.SetActive(false);
    }

    public int GetHp() => currentHealth;
    public void SetHp(int hp) => currentHealth = hp;
}