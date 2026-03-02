using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{
    public GameObject levelSelectPanel;
    public TMP_InputField levelInputField;
    public TextMeshProUGUI errorText;

    public void OpenLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        errorText.text = "";
    }

    public void GoToLevel()
    {
        int level;
        bool isNumber = int.TryParse(levelInputField.text, out level);

        if (!isNumber || level < 1 || level > 4)
        {
            errorText.text = "Enter a number between 1 and 4!";
            return;
        }

        SceneManager.LoadScene("Level" + level);
    }

    public void ClosePanel()
    {
        levelSelectPanel.SetActive(false);
    }
}