using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPulse : MonoBehaviour
{
    [SerializeField] private Light2D light;
    [SerializeField] private float frequency;
    [SerializeField] private float from;
    [SerializeField] private float to;

    void Update()
    {
        float span = (to - from);
        light.intensity = from + span * 0.5f * (1.0f + Mathf.Sin(frequency * (float)Time.time * 2.0f * Mathf.PI));
    }
}
