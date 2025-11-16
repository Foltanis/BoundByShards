using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class FreezeAbility : MonoBehaviour
{
    [SerializeField] private float freezeCooldown = 2f;
    [SerializeField] private float freezeDuration = 3.2f;
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

        var moveAction = input.actions["Move"];
        if (moveAction != null)
        {
            float move = moveAction.ReadValue<float>();
            Debug.Log("Move value: " + move);
        }

        if (freezeAction.triggered)
            Debug.Log("Freeze action triggered");   
    }

    private void CastFreeze()
    {
        if (cooldown) return;

        GameObject freezeArea = Instantiate(freezeZone, transform.position, Quaternion.identity);
        Destroy(freezeArea, 0.2f); // active for a moment

        cooldown = true;
        timer = freezeCooldown;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("Freeze hit: " + other.name);
    //    var freezable = other.GetComponent<Freezable>();
    //    if (freezable != null && other.gameObject != gameObject)
    //        freezable.Freeze(freezeDuration);
    //}
}
