using UnityEngine;

public class EyeOnFreeze : MonoBehaviour, IFreezableReceiver
{
    private EyeAbility eyeAbility;

    // main skript is on the child object but I need colider and prite of parent
    // so this just forwards the calls
    private void Awake()
    {
        eyeAbility = GetComponentInChildren<EyeAbility>();
    }

    public void CastOnFreeze()
    {
        if (eyeAbility != null)
        {
            eyeAbility.CastOnFreeze();
        }
    }

    public void CastOnUnfreeze()
    {
        if (eyeAbility != null)
        {
            eyeAbility.CastOnUnfreeze();
        }
    }
}
