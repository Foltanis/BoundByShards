using UnityEngine;

[CreateAssetMenu(menuName = "Spells/SplitSpellData")]
public class SplitSpellData : ScriptableObject
{
    public float mergeDistance = 0.8f;
    public float splitDistance = 0.4f;
    public float timeToReconnect = 100f;
}
