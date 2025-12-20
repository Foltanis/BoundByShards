using UnityEngine;

public class Gem : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !IsCollected)
        {
            IsCollected = true;
            Debug.Log("Gem collected!");
            SoundManager.PlaySound(SoundType.GEM_COLLECT, gameObject, 1);
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            
            Destroy(gameObject, 10f);
        }
    }
}
