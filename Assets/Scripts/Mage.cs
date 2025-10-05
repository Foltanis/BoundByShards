using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using System.Collections;

public class Mage : PlayerMovement
{
    [SerializeField] private SlimesConn slimes;
    [SerializeField] private SlimeOne slimeOne;
    [SerializeField] private SlimeTwo slimeTwo;

    private InputAction splitSlimes;

    bool slimesSplit = false;
    protected override void Awake()
    {
        base.Awake();
        speed *= 0.3f;
        jumpSpeed *= 0.2f;
        wallJumpSpeed = 0;

        splitSlimes = playerInput.actions["Split"];
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        if (splitSlimes.triggered)
        {
            if (slimesSplit && SlimesCanConnect())
            {
                ConnectSlimes();
            }
            else if (!slimesSplit)
            {
                slimesSplit = true;
                CastSplitSlimes();
            }
        }
    }

    public override void Jump()
    {
        if (grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }

    private void CastSplitSlimes()
    {
        slimesSplit = true;
        Vector3 slimesPos = slimes.GetCurrPosition();
        int slimesHealth = slimes.GetHp();

        slimes.gameObject.SetActive(false);

        slimeOne.gameObject.SetActive(true);
        slimeTwo.gameObject.SetActive(true);

        slimeOne.SetHp(slimesHealth / 2);
        slimeTwo.SetHp(slimesHealth / 2);

        slimeOne.transform.position = slimesPos + new Vector3(-0.4f, 0, 0);
        slimeTwo.transform.position = slimesPos + new Vector3(0.4f, 0, 0);

        StartTimer(10f);
    }
    //TODO move timer to new class where split spell will be handled

    private Coroutine timerCoroutine; // uložíme si referenciu na bežiaci časovač
    private bool timerRunning = false;

    public void StartTimer(float duration)
    {
        if (!timerRunning)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine(duration));
        }
    }

    public void StopTimer()
    {
        if (timerRunning && timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerRunning = false;
            Debug.Log("⏹️ Časovač bol ukončený skôr!");
        }
    }

    private IEnumerator TimerCoroutine(float duration)
    {
        timerRunning = true;
        float time = duration;

        while (time > 0f)
        {
            Debug.Log($"⏱️ zoSTAVA {time:F0} sekúnd");
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }

        Debug.Log("✅ Časovač skončil!");
        timerRunning = false;
        OnTimerEnd();
    }

    private void OnTimerEnd()
    {
        
        if (slimesSplit && SlimesCanConnect())
        {
            ConnectSlimes();
        }
        else
        {
            slimeOne.gameObject.SetActive(false);
            slimeTwo.gameObject.SetActive(false);
            gameObject.SetActive(false); 
        }
    }
    private bool SlimesCanConnect()
    {//TODO check if something isnt between slimes - maybe is not needed because of small distance
        float distance = Vector3.Distance(slimeOne.transform.position, slimeTwo.transform.position);
        return distance <= 0.4f;
    }

    private void ConnectSlimes()
    {
        StopTimer();
        slimesSplit = false;
        Vector3 mergePos = (slimeOne.transform.position + slimeTwo.transform.position) / 2f;
        int newHp = slimeOne.GetHp() + slimeTwo.GetHp();

        slimeOne.gameObject.SetActive(false);
        slimeTwo.gameObject.SetActive(false);

        slimes.gameObject.SetActive(true);
        slimes.SetHp(newHp);
        slimes.transform.position = mergePos;
    }
}
