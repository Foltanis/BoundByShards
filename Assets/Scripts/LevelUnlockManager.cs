using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    [Header("Secret Key Sequence")]
    private KeyCode[] secretSequence = { KeyCode.U, KeyCode.N, KeyCode.L, KeyCode.O, KeyCode.C, KeyCode.K };
    private int currentIndex = 0;

    [Header("UI")]
    public GameObject allLevelsButton;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("LevelsUnlocked");
        // Keep button hidden initially
        allLevelsButton.SetActive(false);

        // If already unlocked in a previous session, show it
        if (PlayerPrefs.GetInt("LevelsUnlocked", 0) == 1)
        {
            allLevelsButton.SetActive(true);
        }
    }

    private void Update()
    {
        DetectSecretSequence();
    }

    private void DetectSecretSequence()
    {
        // Check if the next expected key was pressed
        if (Input.GetKeyDown(secretSequence[currentIndex]))
        {
            currentIndex++;

            // Full sequence entered!
            if (currentIndex >= secretSequence.Length)
            {
                UnlockAllLevels();
                currentIndex = 0;
            }
        }
        else if (Input.anyKeyDown)
        {
            // Wrong key — reset
            currentIndex = 0;
        }
    }

    private void UnlockAllLevels()
    {
        allLevelsButton.SetActive(true);
        PlayerPrefs.SetInt("LevelsUnlocked", 1);
        PlayerPrefs.Save();
        Debug.Log("Secret unlocked! All levels available.");
    }
}