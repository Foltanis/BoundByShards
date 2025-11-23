using UnityEngine;
using System.Collections;

public class SplitSpellController : MonoBehaviour
{
    [SerializeField] private SplitSpellData data;

    [SerializeField] private GameObject slimeOne;
    [SerializeField] private GameObject slimeTwo;
    [SerializeField] private GameObject slimesConnected;

    private bool isSplit = false;
    private Coroutine reconnectTimer;

    private CharacterManager characterManager;

    public void Awake()
    {
        characterManager = CharacterManager.Instance;
    }

    public void Cast()
    {
        if (!isSplit)
            Split();
        else
            TryReconnect();
    }

    private void Split()
    {
        isSplit = true;

        Vector3 pos = slimesConnected.transform.position;
        int hp = slimesConnected.GetComponent<Health>().GetHp();

        characterManager.ActiveCharacters.Remove(slimesConnected);
        characterManager.ActiveCharacters.Add(slimeOne);
        characterManager.ActiveCharacters.Add(slimeTwo);

        slimesConnected.SetActive(false);
        slimeOne.SetActive(true);
        slimeTwo.SetActive(true);

        if (hp % 2 != 0)
            hp += 1; // make even for splitting

        slimeOne.GetComponent<Health>().SetHp(hp / 2);
        slimeTwo.GetComponent<Health>().SetHp(hp / 2);

        slimeOne.transform.position = pos + new Vector3(-data.mergeDistance, 0, 0);
        slimeTwo.transform.position = pos + new Vector3(data.mergeDistance, 0, 0);

        reconnectTimer = StartCoroutine(ReconnectTimer());
    }

    private IEnumerator ReconnectTimer()
    {
        float time = data.timeToReconnect;
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }

        if (TryReconnect())
            yield break;

        // failed reconnect
        slimeOne.SetActive(false);
        slimeTwo.SetActive(false);
        slimesConnected.SetActive(false);
    }

    private void Connect()
    {
        if (reconnectTimer != null)
            StopCoroutine(reconnectTimer);

        int hp = slimeOne.GetComponent<Health>().GetHp() + slimeTwo.GetComponent<Health>().GetHp();
        Vector3 mergePos = (slimeOne.transform.position + slimeTwo.transform.position) / 2f;

        characterManager.ActiveCharacters.Add(slimesConnected);
        characterManager.ActiveCharacters.Remove(slimeOne);
        characterManager.ActiveCharacters.Remove(slimeTwo);

        slimeOne.SetActive(false);
        slimeTwo.SetActive(false);

        slimesConnected.transform.position = mergePos;
        slimesConnected.GetComponent<Health>().SetHp(hp);
        slimesConnected.SetActive(true);

        isSplit = false;
    }

    private bool TryReconnect()
    {
        // manually triggered
        if (Vector3.Distance(slimeOne.transform.position, slimeTwo.transform.position) <= data.mergeDistance)
        {
            Connect();
            return true;
        }
        return false;
    }
        
}
