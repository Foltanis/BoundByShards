using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SplitSpellController : MonoBehaviour
{
    [SerializeField] private SplitSpellData data;
    [SerializeField] private UICooldowns uiCooldowns;

    private GameObject slimesConnected;
    private GameObject slimeOne;
    private PlayerController slimeOnePC;
    private GameObject slimeTwo;

    private bool isSplit = false;
    private Coroutine reconnectTimer;

    private CharacterManager characterManager;

    public void Start()
    {
        characterManager = CharacterManager.Instance;

        slimesConnected = characterManager.Get(CharacterType.SlimesConn);
        slimeOne = characterManager.Get(CharacterType.DashSlime);
        slimeTwo = characterManager.Get(CharacterType.SlimeTwo);

        slimeOnePC = slimeOne.GetComponent<PlayerController>();
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
        uiCooldowns.StartCooldown(UICooldowns.AbilityType.Split, data.timeToReconnect);

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

        slimeOne.transform.position = pos + new Vector3(-data.splitDistance, 0, 0);
        slimeTwo.transform.position = pos + new Vector3(data.splitDistance, 0, 0);

        reconnectTimer = StartCoroutine(ReconnectTimer());

        HashSet<GameObject> characters = CharacterManager.Instance.ActiveCharacters;
        foreach (var character in characters)
        {
            SoundManager.StopSound(SoundType.SLIME_WALK, character);
        }

        SoundManager.PlaySound(SoundType.SPLIT_SPELL, gameObject, 1);
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
        slimeOne.GetComponent<Health>().TakeDamage(9999);
        slimeTwo.GetComponent<Health>().TakeDamage(9999);
    }

    private void Connect()
    {
        if (reconnectTimer != null)
            StopCoroutine(reconnectTimer);

        uiCooldowns.AbilityReady(UICooldowns.AbilityType.Split);

        // fixing bug when slime is still frozen after connecting and splitting again
        if (slimeOnePC.IsFrozen())
        {
            slimeOnePC.CastOnUnfreeze();
            Debug.Log("Unfreezing slime one on reconnect");
            slimeOnePC.SetOriginalColor();
            Debug.Log("Set slime one color to original on reconnect");
        }

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
        
        SoundManager.PlaySound(SoundType.SPLIT_SPELL_JOIN, gameObject, 1);
        HashSet<GameObject> characters = CharacterManager.Instance.ActiveCharacters;
        foreach (var character in characters)
        {
            SoundManager.StopSound(SoundType.SLIME_WALK, character);
        }
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
