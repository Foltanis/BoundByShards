using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    private GameObject[] allPlayers;                      // All players found at Start
    private HashSet<GameObject> playersInside = new HashSet<GameObject>();   // Active players currently inside

    private void Start()
    {
        // Find ALL players in the scene with tag "Player" (active or inactive)
        allPlayers = Resources.FindObjectsOfTypeAll<GameObject>();

        List<GameObject> playerList = new List<GameObject>();
        foreach (var obj in allPlayers)
        {
            if (obj.CompareTag("Player"))
                playerList.Add(obj);
        }

        allPlayers = playerList.ToArray();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter");
        if (!other.CompareTag("Player"))
            return;

        GameObject player = other.gameObject;

        // Only track active players
        if (player.activeInHierarchy)
            playersInside.Add(player);

        // Check if all ACTIVE players are inside
        if (AreAllActivePlayersInside())
            LoadNextScene();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit");
        if (!other.CompareTag("Player"))
            return;

        GameObject player = other.gameObject;

        playersInside.Remove(player);
    }

    private bool AreAllActivePlayersInside()
    {
        foreach (var p in allPlayers)
        {
            if (p.activeInHierarchy && !playersInside.Contains(p))
                return false;
        }
        return true;
    }

    private void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.LogWarning("No next scene in build settings!");
    }
}
