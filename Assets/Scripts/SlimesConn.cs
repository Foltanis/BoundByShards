using UnityEngine;
using UnityEngine.InputSystem;

public class SlimesConn : SlimeBase
{
    protected override void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;
        currentHealth = maxHealth;

        playerInput = GetComponent<PlayerInput>();
        speed *= 0.65f;
        jumpSpeed *= 0.65f;
        wallJumpSpeed *= 0.5f;

        maxHealth *= 2;
    }
    protected override float GetHorizontalInput()
    {
        float input = 0;
        if (Input.GetKey(KeyCode.Keypad4)) input = -1;
        if (Input.GetKey(KeyCode.Keypad6)) input = 1;
        return input;
    }

    protected override bool GetJumpKey()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public override void SpecialAbility()
    {
        Debug.Log("None");
    }

    public Vector3 GetCurrPosition()
    {
        return transform.position;
    }

}
