using UnityEngine;

public static class GameLoader
{
    public static void Load(string id)
    {
        var data = SaveSystem.LoadGameData(id);
        if (data == null)
        {
            Debug.LogWarning($"Failed to load save with id {id}");
            return;
        }

        Debug.Log($"Loaded save '{data.metadata.displayName}' successfully.");
    }
}
