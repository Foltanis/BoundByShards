using UnityEngine;
public abstract class SlimeBase : PlayerMovement
{
    protected override void HandleInput()
    {
        float horizontalInput = GetHorizontalInput();
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);


        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);

        if (GetJumpKey() && grounded)
            Jump();
        else if (GetJumpKey() && onWall)
            Jump();
    }

   
    protected abstract float GetHorizontalInput();
    protected abstract bool GetJumpKey();

   
    public abstract void SpecialAbility();
}
