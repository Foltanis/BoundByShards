using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void SaveGame()
    {
        string id = PlayerPrefs.GetString("currentSaveId");
        GameData data = SaveSystem.LoadGameData(id);
        data.levelSceneName = SceneManager.GetActiveScene().name;
        SaveSystem.SaveGameData(data);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
