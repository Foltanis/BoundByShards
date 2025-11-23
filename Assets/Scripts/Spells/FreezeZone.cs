using UnityEngine;

public class FreezeZone : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 3f;
    [SerializeField] private ParticleSystem freezeParticles;

    void Start()
    {
        if (freezeParticles != null)
        {
            var ps = Instantiate(freezeParticles, transform.position, Quaternion.identity);
            ps.Play();

            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }
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
