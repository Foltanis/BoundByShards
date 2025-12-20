using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private CharacterManager characterManager;
    private List<GameObject> gems;

    private HashSet<GameObject> playersInside = new HashSet<GameObject>();

    private void Start()
    {
        characterManager = CharacterManager.Instance;
        gems = GameObject.FindGameObjectsWithTag("Gem").ToList();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!characterManager.ActiveCharacters.Contains(other.gameObject))
            return;

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
        // Count uncollected gems
        int remainingGems = gems.Count(g => g != null && !g.GetComponent<Gem>().IsCollected);

        int totalPlayers = characterManager.ActiveCharacters.Count;
        int insidePlayers = playersInside.Count;

        if (totalPlayers > 0 && insidePlayers == totalPlayers)
        {
            if (remainingGems > 0)
                Debug.Log($"{remainingGems} gem(s) not collected!");
            else
            {
                Debug.Log("All players in zone! Loading next scene...");
                SceneTransitionManager.Instance.LoadNextScene();
            }
        }
    }

}
