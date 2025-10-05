using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public static class SaveSystem
{
    private static string saveDir => Path.Combine(Application.persistentDataPath, "Saves");
    private static string indexPath => Path.Combine(saveDir, "index.json");

    private static Dictionary<string, SaveMetadata> saves = new();

    public static void Init()
    {
        if (!Directory.Exists(saveDir))
            Directory.CreateDirectory(saveDir);

        if (File.Exists(indexPath))
        {
            string json = File.ReadAllText(indexPath);
            saves = JsonUtility.FromJson<SaveMetadataList>(json).ToDictionary();
        }
    }

    public static void CreateNewGame(string displayName)
    {
        string id = Guid.NewGuid().ToString();
        var meta = new SaveMetadata
        {
            id = id,
            displayName = displayName,
            creationTime = DateTime.Now.ToString(),
            lastPlayedTime = DateTime.Now.ToString(),
            playDuration = 0f
        };
        saves[id] = meta;
        SaveMetadataIndex();

        PlayerPrefs.SetString("currentSaveId", id);

        GameData newData = new GameData { metadata = meta };
        SaveGameData(newData);
    }

    public static void SaveGameData(GameData data)
    {
        string path = Path.Combine(saveDir, data.metadata.id + ".save");

        data.metadata.lastPlayedTime = DateTime.Now.ToString();

        using FileStream stream = new(path, FileMode.Create);
        BinaryFormatter formatter = new();
        formatter.Serialize(stream, data);

        saves[data.metadata.id] = data.metadata;
        SaveMetadataIndex();
    }

    public static GameData LoadGameData(string id)
    {
        string path = Path.Combine(saveDir, id + ".save");
        if (!File.Exists(path)) return null;

        using FileStream stream = new(path, FileMode.Open);
        BinaryFormatter formatter = new();
        return formatter.Deserialize(stream) as GameData;
    }

    public static void DeleteSave(string id)
    {
        string path = Path.Combine(saveDir, id + ".save");
        if (File.Exists(path)) File.Delete(path);

        if (saves.Remove(id))
            SaveMetadataIndex();
    }

    private static void SaveMetadataIndex()
    {
        var wrapper = new SaveMetadataList { list = new List<SaveMetadata>(saves.Values) };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(indexPath, json, Encoding.UTF8);
    }

    [Serializable]
    private class SaveMetadataList
    {
        public List<SaveMetadata> list;
        public Dictionary<string, SaveMetadata> ToDictionary() =>
            list.ToDictionary(m => m.id, m => m);
    }

    public static List<SaveMetadata> GetAllMetadata()
    {
        return new List<SaveMetadata>(saves.Values);
    }
}
