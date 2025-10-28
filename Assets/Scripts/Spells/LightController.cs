using UnityEngine;
using UnityEngine.InputSystem;

public class LightController : MonoBehaviour
{
    private InputAction moveAction;
    [SerializeField] private float speed = 5f;

    
    public void Init(InputAction moveInput)
    {
        moveAction = moveInput;
        moveAction.Enable();
    }


    private void Update()
    {
        if (moveAction == null) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, input.y, 0) * speed * Time.deltaTime;
        transform.position += move;
    }
}
