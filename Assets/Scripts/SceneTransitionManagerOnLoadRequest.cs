using UnityEngine;

public class SceneTransitionManagerOnLoadRequest : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private float volume = 0.7f;
    [SerializeField] private float startTime = 0f;
    
    [Header("Manager Prefab (for standalone scene testing)")]
    [SerializeField] private GameObject sceneTransitionManagerPrefab;

    void Start()
    {
        // Ensure the manager exists
        if (SceneTransitionManager.Instance == null)
        {
            if (sceneTransitionManagerPrefab != null)
            {
                Instantiate(sceneTransitionManagerPrefab);
            }
            else
            {
                // Create one from scratch if no prefab assigned
                var go = new GameObject("SceneTransitionManager");
                go.AddComponent<SceneTransitionManager>();
            }
        }
        
        var manager = SceneTransitionManager.Instance;
        manager.SetMusic(music, volume, startTime);
        
        if (!manager.IsTransitioning)
        {
            manager.SetMusicVolumeImmediate(volume);
        }
        
        manager.SceneReady();
    }
}