using UnityEngine;

public class SceneTransitionManagerOnLoadRequest : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private float volume = 0.7f;
    [SerializeField] private float startTime = 0f;

    void Start()
    {
        var manager = SceneTransitionManager.Instance;
        manager.SetMusic(music, volume, startTime);
        
        // If not transitioning, we're the first scene - start music at full volume
        if (!manager.IsTransitioning)
        {
            manager.SetMusicVolumeImmediate(volume);
        }
        
        manager.SceneReady();
    }
}