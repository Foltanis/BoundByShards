using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (CompareTag("Player"))
        {
            Debug.Log("Player died. Reloading scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetHp() => currentHealth;
    public void SetHp(int hp) => currentHealth = hp;
}