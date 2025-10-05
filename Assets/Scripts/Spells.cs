using UnityEngine;

public class Spells : MonoBehaviour
{
    protected SlimesConn slimes;
    protected SlimeOne slimeOne;
    protected SlimeTwo slimeTwo;
    protected Mage mage;

    public virtual void Awake()
    {
        slimeOne = FindAnyObjectByType<SlimeOne>(FindObjectsInactive.Include);
        slimeTwo = FindAnyObjectByType<SlimeTwo>(FindObjectsInactive.Include);
        slimes = FindAnyObjectByType<SlimesConn>(FindObjectsInactive.Include);
        mage = FindAnyObjectByType<Mage>();
    }
    public virtual void Cast()
    {
        Debug.Log("Base spell casted!");
    }
}
