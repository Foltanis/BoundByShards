using UnityEngine;
using System.Collections;

public class Freezable : MonoBehaviour
{
    private bool frozen = false;
    public bool IsFrozen => frozen;

    public void Freeze(float duration)
    {
        if (!frozen)
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
                receiver.OnFreeze();
            else
                receiver.OnUnfreeze();
        }
    }
}
