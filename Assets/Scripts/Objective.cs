using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;

    private bool player1_achieved = false;
    private bool player2_achieved = false;
    private bool player3_achieved = false;

    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player3 = GameObject.Find("Player3");
    }

    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player1":
                player1_achieved = true;
                break;
            case "Player2":
                player2_achieved = true;
                break;
            case "Player3":
                player3_achieved = true;
                break;
            default:
                return;
        }

        if (player1_achieved && player2_achieved && player3_achieved)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextIndex);
            else
                Debug.LogWarning("No next scene in build settings!");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player1":
                player1_achieved = false;
                break;
            case "Player2":
                player2_achieved = false;
                break;
            case "Player3":
                player3_achieved = false;
                break;
            default:
                return;
        }
    }
}
