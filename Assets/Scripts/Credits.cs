using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    void Start()
    {
        audioSource.time = 175.8f;
    }
}
