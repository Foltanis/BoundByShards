using UnityEngine;
using System;
using System.Collections.Generic;

public enum SoundType
{
    CANDLE_LIT,
    EYE_FLYING,
    GOLEM_ATTACK,
    GOLEM_DEATH,
    GOLEM_IDLE,
    MAGE_IDLE,
    MAGE_JUMP,
    MAGE_WALK,
    OCCULTIST_IDLE,
    OCCULTIST_WALK,
    SLIME_IDLE,
    SLIME_JUMP,
    SLIME_WALK,
    TURNIP_ATTACK,
    TURNIP_IDLE,
    TURNIP_JUMP,
    TURNIP_JUMP_IN,
    TURNIP_WALK,
    FIREBALL,
    SPLIT_SPELL,
    LIGHT_SPELL,
    SLIME_DASH,
    SLIME_FREEZE,
    SPLIT_SPELL_JOIN,
    WATERFALL,
    GEM_COLLECT,
    FIREBALL_HIT,
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundList[] soundList;
    
    private Dictionary<(SoundType, GameObject), List<AudioSource>> activeSounds = new();

#if UNITY_EDITOR
    private void OnEnable() {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++) {
            soundList[i].name = names[i];
        }
    }
#endif

    void Awake()
    {
        if (!Application.isPlaying)
            return;
            
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!Application.isPlaying) return;
        
        // Clean up finished non-looping sounds
        var toRemove = new List<(SoundType, GameObject)>();
        
        foreach (var kvp in activeSounds)
        {
            kvp.Value.RemoveAll(source => {
                if (source == null) return true;
                if (!source.isPlaying && !source.loop)
                {
                    Destroy(source);
                    return true;
                }
                return false;
            });
            
            if (kvp.Value.Count == 0)
                toRemove.Add(kvp.Key);
        }
        
        foreach (var key in toRemove)
            activeSounds.Remove(key);
    }

    public static void PlaySound(SoundType sound, GameObject owner = null, float volume = 1f, bool loop = false)
    {
        AudioClip clip = Instance.soundList[(int)sound].sound;
        
        AudioSource source = Instance.gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.Play();
        
        var key = (sound, owner);
        if (!Instance.activeSounds.ContainsKey(key))
            Instance.activeSounds[key] = new List<AudioSource>();
        
        Instance.activeSounds[key].Add(source);
    }

    public static void StopSound(SoundType sound, GameObject owner = null)
    {
        var key = (sound, owner);
        if (Instance.activeSounds.TryGetValue(key, out var sources))
        {
            foreach (var source in sources)
            {
                if (source != null)
                {
                    source.Stop();
                    Destroy(source);
                }
            }
            Instance.activeSounds.Remove(key);
        }
    }
    
    public static void StopAllSounds(GameObject owner)
    {
        var toRemove = new List<(SoundType, GameObject)>();
        
        foreach (var kvp in Instance.activeSounds)
        {
            if (kvp.Key.Item2 == owner)
            {
                foreach (var source in kvp.Value)
                {
                    if (source != null)
                    {
                        source.Stop();
                        Destroy(source);
                    }
                }
                toRemove.Add(kvp.Key);
            }
        }
        
        foreach (var key in toRemove)
            Instance.activeSounds.Remove(key);
    }
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name;
    [SerializeField] public AudioClip sound;
}
