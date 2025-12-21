using UnityEngine;
using TMPro;
using System;

public class SaveItemUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    private SaveMetadata meta;

    public void Setup(SaveMetadata data)
    {
        meta = data;
        nameText.text = data.displayName;
    }

    public void OnDelete() => Destroy(gameObject);

    public void OnLoadClicked() => GameLoader.Load(meta.id);
    public void OnDeleteClicked() => SaveSystem.DeleteSave(meta.id);
}
