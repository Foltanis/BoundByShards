using UnityEngine;
using TMPro;
using System;

public class SaveItemUI : MonoBehaviour
{
    public TextMeshProUGUI nameText, timeText, durationText;
    private SaveMetadata meta;

    public void Setup(SaveMetadata data)
    {
        meta = data;
        nameText.text = data.displayName;
        timeText.text = $"Created: {data.creationTime}\nLast Played: {data.lastPlayedTime}";
        durationText.text = $"{TimeSpan.FromSeconds(data.playDuration):hh\\:mm\\:ss}";
    }

    public void OnLoadClicked() => GameLoader.Load(meta.id);
    public void OnDeleteClicked() => SaveSystem.DeleteSave(meta.id);
}
