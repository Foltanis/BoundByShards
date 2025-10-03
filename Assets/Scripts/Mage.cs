using UnityEngine;

public class Mage : PlayerMovement
{
    protected override void Awake()
    {
        base.Awake();
        speed *= 0.4f;  
        wallJumpSpeed = 0;
    }

    protected override void HandleInput()
    {
        float horizontalInput = 0;
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1;
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1;

        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);


        if (Input.GetKeyDown(KeyCode.W))
            Jump();
    }

    public override void Jump()
    {
        if (grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }
}
