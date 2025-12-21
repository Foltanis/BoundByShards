using UnityEngine;
using System.Collections;

public class Freezable : MonoBehaviour
{
    // frozen bool maybe not needed or use it in other classes TODO
    private bool frozen = false;
    public bool IsFrozen => frozen;

    private SpriteRenderer sr;
    private Color originalColor;
    MaterialPropertyBlock block;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    public void Freeze(float duration)
    {
        StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float time)
    {
        frozen = true;
        NotifyFreezeReceivers(true);

        yield return new WaitForSeconds(time);

        frozen = false;
        NotifyFreezeReceivers(false);
    }

    private void NotifyFreezeReceivers(bool freezing)
    {
        foreach (var receiver in GetComponents<IFreezableReceiver>())
        {
            Debug.Log($"Notifying receiver {receiver} of freeze state: {freezing}");

            if (freezing)
            {
                DefaultFreezeEfect();
                receiver.CastOnFreeze();
            }
            else
            {
                DefaultUnfreezeEfect();
                receiver.CastOnUnfreeze();
            }
                
        }
    }

    private void DefaultFreezeEfect()
    {
        sr.color = new Color(0.5f, 0.8f, 1f);
    }

    private void DefaultUnfreezeEfect()
    {
        sr.color = originalColor;        
    }
}
