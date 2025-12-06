using UnityEngine;
using UnityEngine.InputSystem;

public class MageCarryHandler : MonoBehaviour
{
    private CarryAbility currentCarrier;
    private InputAction moveInput;

    private void Awake()
    {
        moveInput = GetComponent<PlayerInput>().actions["Move"];
    }
    public void SetCarrier(CarryAbility carrier)
    {
        currentCarrier = carrier;
    }

    private void Update()
    {
        if (currentCarrier == null) return;

        // Check for movement input
        float movement = moveInput.ReadValue<float>();

        // if mage wants to move, drop the mage
        if (Mathf.Abs(movement) > 0.01)
        {
            Debug.Log("Mage is trying to move!");
            currentCarrier.DropMage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if mage collides with something while being carried, drop the mage
        if (currentCarrier != null)
        {
            currentCarrier.DropMage();
        }

        currentCarrier = null;
    }
}
