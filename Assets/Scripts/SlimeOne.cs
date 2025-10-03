using UnityEngine;

public class SlimeOne : SlimeBase
{
    protected override float GetHorizontalInput()
    {
        float input = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) input = -1;
        if (Input.GetKey(KeyCode.RightArrow)) input = 1;
        return input;
    }

    protected override bool GetJumpKey()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public override void SpecialAbility()
    {
        Debug.Log("kraaaaa");
    }
}
