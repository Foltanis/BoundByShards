using UnityEngine;

public class MageLight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var turnip = other.GetComponent<PursuingAbility>();
        Debug.Log("MageLight hit: " + other.name);
        if (turnip != null)
        {
            turnip.Stun();
            Debug.Log("Stunned turnip: " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var turnip = other.GetComponent<PursuingAbility>();
        if (turnip != null)
        {
            turnip.Unstun();
            Debug.Log("Unstunned turnip: " + other.name);
        }
    }
}

