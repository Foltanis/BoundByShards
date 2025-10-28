using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterEntry
{
    public string id;
    public GameObject instance;
    public bool activeOnStart = true;
}

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] private List<CharacterEntry> characters = new List<CharacterEntry>();

    private Dictionary<string, GameObject> lookup = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // persist across scenes with characters

        foreach (var entry in characters)
        {
            if (!lookup.ContainsKey(entry.id))
                lookup.Add(entry.id, entry.instance);

            if (!entry.activeOnStart && entry.instance != null)
                entry.instance.SetActive(false);

        }
    }

    public void Spawn(string id, Vector3 position)
    {
        if (lookup.TryGetValue(id, out var obj) && obj != null)
        {
            obj.transform.position = position;
            obj.SetActive(true);
            return;
        }

        Debug.LogWarning($"❌ Character '{id}' not found!");
    }

    public GameObject GetPrefab(string id)
    {
        if (lookup.TryGetValue(id, out var prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"❌ Character '{id}' not found!");
        return null;
    }
}
