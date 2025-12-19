using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    
    private AudioSource musicSource;
    private Image fadeImage;
    private bool isTransitioning;
    private float musicVolume;
    private bool waitingForSceneSetup;
    public bool IsTransitioning => isTransitioning;

    public void SetMusicVolumeImmediate(float volume)
    {
        musicVolume = volume;
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        CreateFadeCanvas();
        
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
    }

    void CreateFadeCanvas()
    {
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);
        
        Canvas fadeCanvas = canvasObj.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 999;
        
        canvasObj.AddComponent<CanvasScaler>();
        
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform);
        
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = false;
        
        RectTransform rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        SetFadeAlpha(0);
    }

    public void SetMusic(AudioClip clip, float volume = 0.7f, float startTime = 0f)
    {
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;
            
        musicVolume = volume;
        musicSource.clip = clip;
        musicSource.volume = 0f; // Start silent, fade will bring it up
        
        if (clip != null)
        {
            musicSource.time = Mathf.Clamp(startTime, 0f, clip.length);
            musicSource.Play();
        }
    }

    // Called by SceneTransitionManagerOnLoadRequest after setting music
    public void SceneReady()
    {
        waitingForSceneSetup = false;
        
        if (!isTransitioning)
        {
            StartCoroutine(Fade(1, 0, 0, musicVolume));
        }
    }

    public void LoadScene(int buildIndex)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScene(buildIndex));
        }
    }

    public void LoadNextScene()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
        {
            LoadScene(next);
        }
    }

    IEnumerator TransitionToScene(int buildIndex)
    {
        isTransitioning = true;

        yield return StartCoroutine(Fade(0, 1, musicVolume, 0));
        

        waitingForSceneSetup = true;
        SceneManager.LoadScene(buildIndex);
        
        while (waitingForSceneSetup)
        {
            yield return null;
        }
        
        yield return StartCoroutine(Fade(1, 0, 0, musicVolume));
        
        isTransitioning = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float startVol, float endVol)
    {
        float elapsed = 0;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeDuration;
            
            SetFadeAlpha(Mathf.Lerp(startAlpha, endAlpha, t));
            
            if (musicSource != null)
            {
                musicSource.volume = Mathf.Lerp(startVol, endVol, t);
            }
            
            yield return null;
        }
        
        SetFadeAlpha(endAlpha);
        if (musicSource != null) musicSource.volume = endVol;
    }

    void SetFadeAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}
