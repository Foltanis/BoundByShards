using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEditor;

public class SceneFadeOut : MonoBehaviour
{
    [SerializeField] private SceneAsset sceneAfterMusicEnd;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 3f;

    public void FadeAndChangeScene()
    {
        StartCoroutine(FadeBeforeSceneChange());
        if (canvasGroup) StartCoroutine(FadeUI());
    }

    private System.Collections.IEnumerator FadeBeforeSceneChange()
    {
        yield return StartCoroutine(FadeOutScene());
        SceneManager.LoadScene(sceneAfterMusicEnd.name);
    }

    private System.Collections.IEnumerator FadeOutScene()
    {
        float startVolume = audioSource.volume;
        float startLightIntensity = globalLight.intensity;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, normalized);
            globalLight.intensity = Mathf.Lerp(startLightIntensity, 0f, normalized);
            yield return null;
        }
        audioSource.volume = 0f;
        globalLight.intensity = 0f;
    }

    private System.Collections.IEnumerator FadeUI()
    {
        float t = 0f;
        float start = canvasGroup.alpha;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
