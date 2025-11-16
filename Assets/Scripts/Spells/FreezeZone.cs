using UnityEngine;

public class FreezeZone : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("FreezeZone hit: " + other.name);
        var freezable = other.GetComponent<Freezable>();
        if (freezable != null)
        {
            freezable.Freeze(freezeDuration);
        }
    }
}
