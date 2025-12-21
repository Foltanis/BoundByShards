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
        eyeAbility?.CastOnFreeze();
        // SoundManager.StopSound(SoundType.EYE_FLYING, gameObject);
    }

    public void CastOnUnfreeze()
    {
        eyeAbility?.CastOnUnfreeze();
        // SoundManager.PlaySound(SoundType.EYE_FLYING, gameObject, 0.01f);
    }
}
