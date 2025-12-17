using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeDuration = 2f; // seconds

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            // Exponential curve for natural fade
            audioSource.volume = Mathf.Pow(t, 2f); // try 2–3 for smoother fade

            yield return null;
        }

        audioSource.volume = 1f;
    }
}
