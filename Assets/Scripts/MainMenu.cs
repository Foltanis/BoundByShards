using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField newGameNameInput;

    void Start()
    {
        SaveSystem.Init();
    }

    void Update()
    {
        
    }

    public void NewGame()
    {
        string newGameName = newGameNameInput.text;
        SaveSystem.CreateNewGame(newGameName);
        Debug.Log("Starting new game: " + newGameName);
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
