using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SceneFadeOut sceneFadeOut;
    
    void Start()
    {
        audioSource.time = 175.8f;
        StartCoroutine(WaitForAudioToEnd());
    }

    private System.Collections.IEnumerator WaitForAudioToEnd()
    {
        while (audioSource.isPlaying)
            yield return null;
        sceneFadeOut.FadeAndChangeScene();
    }
}
