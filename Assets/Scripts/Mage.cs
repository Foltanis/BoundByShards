using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Mage : PlayerMovement
{
    protected override void Awake()
    {
        base.Awake();
        speed *= 0.3f;
        jumpSpeed *= 0.2f;
        wallJumpSpeed = 0;
    }


    public override void Jump()
    {
        if (grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }
}
