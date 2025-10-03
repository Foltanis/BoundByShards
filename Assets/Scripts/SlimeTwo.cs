using UnityEngine;

public class SlimeTwo : SlimeBase
{
    protected override float GetHorizontalInput()
    {
        float input = 0;
        if (Input.GetKey(KeyCode.Keypad4)) input = -1;
        if (Input.GetKey(KeyCode.Keypad6)) input = 1;
        return input;
    }

    protected override bool GetJumpKey()
    {
        return Input.GetKeyDown(KeyCode.Keypad8);
    }

    public override void SpecialAbility()
    {
        Debug.Log("fuuuuck");
    }
}

