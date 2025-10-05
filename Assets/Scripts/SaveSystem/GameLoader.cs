using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameLoader
{
    public static void Load(string id)
    {
        var data = SaveSystem.LoadGameData(id);
        if (data == null)
        {
            Debug.LogWarning($"Failed to load save with id {id}");
            return;
        }

        string sceneName = data.levelSceneName;
        PlayerPrefs.SetString("currentSaveId", id);
        SceneManager.LoadScene(sceneName);
    }
}
