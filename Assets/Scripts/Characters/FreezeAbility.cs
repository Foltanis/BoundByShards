using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class FreezeAbility : MonoBehaviour
{
    [SerializeField] private float freezeCooldown = 2f;
    [SerializeField] private GameObject freezeZone;

    private bool cooldown = false;
    private float timer = 0f;

    private PlayerInput input;
    private InputAction freezeAction;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        freezeAction = input.actions["Freeze"];
    }

    private void Update()
    {
        if (cooldown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f) cooldown = false;
        }

        if (freezeAction.triggered)
            CastFreeze();
    }

    private void CastFreeze()
    {
        if (cooldown) return;

        GameObject freezeArea = Instantiate(freezeZone, transform.position, Quaternion.identity);
        Destroy(freezeArea, 0.2f); // active for a moment

        cooldown = true;
        timer = freezeCooldown;
        SoundManager.PlaySound(SoundType.SLIME_FREEZE, gameObject, 1);
    }
}
