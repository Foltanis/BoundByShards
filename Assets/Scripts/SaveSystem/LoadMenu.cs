using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    public Transform contentRoot;
    public GameObject saveItemPrefab;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        foreach (var meta in SaveSystem.GetAllMetadata())
        {
            var item = Instantiate(saveItemPrefab, contentRoot);
            item.GetComponent<SaveItemUI>().Setup(meta);
        }
    }
}
