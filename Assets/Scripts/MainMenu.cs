using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField newGameNameInput;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void NewGame()
    {
        string newGameName = newGameNameInput.text;
        Debug.Log("Starting new game: " + newGameName);
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
