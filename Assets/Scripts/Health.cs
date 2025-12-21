using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    private bool isDead = false;
    private Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log($"{gameObject.name} hp: {currentHealth} took {dmg} damage.");
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("die");
        }
        else
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
        if (gameObject.CompareTag("Player"))
        {
            ReloadScene();
        }
    }

    public void ReloadScene()
    { 
        Debug.Log("Player died. Reloading scene...");
        SceneTransitionManager.Instance.ReloadCurrentScene();
    }

    public int GetHp() => currentHealth;
    public void SetHp(int hp) => currentHealth = hp;
}