using UnityEngine;
using System.Collections.Generic;

public class Cooldowns : MonoBehaviour
{
    [SerializeField] private ProgressBar dashCooldownBar;
    [SerializeField] private ProgressBar freezeCooldownBar;
    [SerializeField] private ProgressBar fireballCooldownBar;
    [SerializeField] private ProgressBar splitCooldownBar;

    public enum AbilityType
    {
        Dash,
        Freeze,
        Fireball,
        Split
    }

    private class CooldownData
    {
        public ProgressBar bar;
        public float duration;
        public float timer;
        public bool active;
    }

    private Dictionary<AbilityType, CooldownData> cooldowns = new();

    void Awake()
    {
        cooldowns[AbilityType.Dash] = new CooldownData
        {
            bar = dashCooldownBar
        };

        cooldowns[AbilityType.Freeze] = new CooldownData
        {
            bar = freezeCooldownBar
        };

        cooldowns[AbilityType.Fireball] = new CooldownData
        {
            bar = fireballCooldownBar
        };

        cooldowns[AbilityType.Split] = new CooldownData
        {
            bar = splitCooldownBar
        };

        foreach (var cd in cooldowns.Values)
        {
            cd.bar.lowValue = 0f;
            cd.bar.highValue = 1f;
            cd.bar.value = 1f;
        }
    }

    void Update()
    {
        foreach (var cd in cooldowns.Values)
        {
            if (!cd.active) continue;

            cd.timer -= Time.deltaTime;

            // normalize timer to [0,1]
            float normalized = 1 - Mathf.Clamp01(cd.timer / cd.duration);
            cd.bar.value = normalized;

            if (cd.timer <= 0f)
            {
                cd.bar.value = 1f;
                cd.active = false;
                // cd.bar.style.opacity = 1f;
            }
        }
    }

    public void StartCooldown(AbilityType type, float duration)
    {
        if (!cooldowns.ContainsKey(type)) return;
        Debug.Log($"Starting cooldown for {type} with duration {duration}s");
        var cd = cooldowns[type];
        cd.duration = duration;
        cd.timer = duration;
        cd.bar.value = 0f;
        cd.active = true;
        // cd.bar.style.opacity = 0.5f;
    }

    public void AbilityReady(AbilityType type)
    {
        if (!cooldowns.ContainsKey(type)) return;
        var cd = cooldowns[type];
        cd.active = true;
        cd.bar.value = 1f;
        // cd.bar.style.opacity = 1f;
    }
}
