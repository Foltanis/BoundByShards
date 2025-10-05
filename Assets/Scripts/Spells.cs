using UnityEngine;

public class Spells : MonoBehaviour
{
    protected SlimesConn slimes;
    protected SlimeOne slimeOne;
    protected SlimeTwo slimeTwo;

    public void Awake()
    {
        slimeOne = FindAnyObjectByType<SlimeOne>(FindObjectsInactive.Include);
        slimeTwo = FindAnyObjectByType<SlimeTwo>(FindObjectsInactive.Include);
        slimes = FindAnyObjectByType<SlimesConn>(FindObjectsInactive.Include);
    }
    public virtual void Cast()
    {
        Debug.Log("Base spell casted!");
    }
}
