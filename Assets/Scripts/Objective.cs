using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    [SerializeField] private GameObject gem;
    private CharacterManager characterManager;

    private HashSet<GameObject> playersInside = new HashSet<GameObject>();

    private void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!characterManager.ActiveCharacters.Contains(other.gameObject))
            return; // ignore non-player objects

        playersInside.Add(other.gameObject);

        CheckObjective();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!characterManager.ActiveCharacters.Contains(other.gameObject))
            return;

        playersInside.Remove(other.gameObject);
    }

    private void CheckObjective()
    {
        int totalPlayers = characterManager.ActiveCharacters.Count;
        int insidePlayers = playersInside.Count;

        if (totalPlayers > 0 && insidePlayers == totalPlayers)
        {
            if (gem != null)
                Debug.Log("Gem not collected!");
            else
            {
                Debug.Log("All players in zone! Loading next scene...");
                SceneTransitionManager.Instance.LoadNextScene();
            }
                
        }
    }
}
