using UnityEngine;
using System.Collections.Generic;


public enum CharacterType
{
    Mage,
    SlimesConn,
    DashSlime,
    SlimeTwo
}

[System.Serializable]
public class CharacterEntry
{
    public CharacterType id;
    public GameObject instance;
    public bool activeOnStart = true;
}

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] private List<CharacterEntry> characters = new List<CharacterEntry>();

    private Dictionary<CharacterType, GameObject> lookup = new Dictionary<CharacterType, GameObject>();

    private HashSet<GameObject> activeCharacters = new HashSet<GameObject>();

    public List<CharacterEntry> Characters => characters;
    public HashSet<GameObject> ActiveCharacters => activeCharacters;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject); // persist across scenes with characters

        foreach (var entry in characters)
        {
            if (!lookup.ContainsKey(entry.id))
                lookup.Add(entry.id, entry.instance);

            if (!entry.activeOnStart && entry.instance != null)
                entry.instance.SetActive(false);

            if (entry.activeOnStart && entry.instance != null)
                activeCharacters.Add(entry.instance);
        }
    }

    public void Spawn(CharacterType id, Vector3 position)
    {
        if (lookup.TryGetValue(id, out var obj) && obj != null)
        {
            obj.transform.position = position;
            obj.SetActive(true);
            return;
        }

        Debug.LogWarning($"Character '{id}' not found!");
    }

    public GameObject Get(CharacterType id)
    {
        if (lookup.TryGetValue(id, out var prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"Character '{id}' not found!");
        return null;
    }
}
