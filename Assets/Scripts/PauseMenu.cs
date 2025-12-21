using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    private PlayerInput playerInput;
    private InputAction uiAction;
    private bool isPaused;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.currentActionMap;
        uiAction = actionMap.FindAction("PauseMenu");

        uiAction.performed += ctx => TogglePause();
    }

    public void TogglePause()
    {
        Debug.Log("Pause menu toggled");
        isPaused = !isPaused;
        
        Time.timeScale = isPaused ? 0f : 1f;
        
        panel.SetActive(isPaused);
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

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.ReloadCurrentScene();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetString("currentSaveId", "");
        SceneTransitionManager.Instance.LoadScene(0);
    }
}