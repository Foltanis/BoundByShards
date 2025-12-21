using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] public float lowValue = 0f;
    [SerializeField] public float highValue = 1f;
    [SerializeField] public float value = 1f;
    [SerializeField] private GameObject barFill;
    [SerializeField] public Color barColor = Color.white;

    void Start()
    {
        UpdateBarFill();
    }

    void Update()
    {
        UpdateBarFill();
    }

    private void UpdateBarFill()
    {
        if (barFill != null)
        {
            float normalizedValue = (value - lowValue) / (highValue - lowValue);
            Vector3 scale = barFill.transform.localScale;
            scale.x = normalizedValue;
            barFill.transform.localScale = scale;
            barFill.GetComponent<UnityEngine.UI.Image>().color = barColor;
            barFill.transform.localPosition = new Vector3((normalizedValue - 1f) / 2f, 0f, 0f);
        }
    }

    // public float LowValue
    // {
    //     get { return lowValue; }
    //     set { lowValue = value; UpdateBarFill(); }
    // }

    // public float HighValue
    // {
    //     get { return highValue; }
    //     set { highValue = value; UpdateBarFill(); }
    // }

    // public float Value
    // {
    //     get { return value; }
    //     set { this.value = value; UpdateBarFill(); }
    // }
}
