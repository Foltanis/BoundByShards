using UnityEngine;

public class FreezeHitbox : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 3.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var freezable = other.GetComponent<Freezable>();
        if (freezable != null)
        {
            Debug.Log("Freeze hit " + other.name);
            freezable.Freeze(freezeDuration);
        }
    }
}
