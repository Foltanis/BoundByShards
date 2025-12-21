using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }
}
