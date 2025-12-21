using UnityEngine;
using UnityEngine.UIElements;

public class DisablePicking : MonoBehaviour
{
    void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        uiDocument.rootVisualElement.pickingMode = PickingMode.Ignore;
    }
}
