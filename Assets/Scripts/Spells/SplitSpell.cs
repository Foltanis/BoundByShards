using UnityEngine;

public class SplitSpell
{
    private static SplitSpell _instance;
    public static SplitSpell Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SplitSpell();
            return _instance;
        }
    }

    private bool slimesSplit = false;
    private float mergeDistance = 0.4f;
    private float timeToConnect = 10f;

    private GameObject dashSlime;
    private GameObject slimeTwo;
    private GameObject connSlimes;

    private Timer timer;


    
    private SplitSpell()
    {
        var cm = CharacterManager.Instance;
        dashSlime = cm.GetPrefab("DashSlime");
        slimeTwo = cm.GetPrefab("SlimeTwo");
        connSlimes = cm.GetPrefab("ConnSlimes");

        timer = new Timer(timeToConnect);
    }

    public void Update(float deltaTime)
    {
        if (!timer.IsRunning)
            return;


        timer.Update(deltaTime);
        if (timer.IsFinished())
            OnTimerEnd();
        
    }

    public void Cast()
    {
        if (slimesSplit && SlimesCanConnect())
            ConnectSlimes();
        else if (!slimesSplit)
            SplitSlimes();
    }

    private void SplitSlimes()
    {
        slimesSplit = true;
        Vector3 pos = connSlimes.transform.position;
        int hp = connSlimes.GetComponent<Health>().GetHp();

        connSlimes.SetActive(false);
        CharacterManager.Instance.Spawn("DashSlime", pos + new Vector3(-mergeDistance, 0, 0));
        CharacterManager.Instance.Spawn("SlimeTwo", pos + new Vector3(mergeDistance, 0, 0));

        dashSlime.GetComponent<Health>().SetHp(hp / 2);
        slimeTwo.GetComponent<Health>().SetHp(hp / 2);

        timer.Reset();
    }

    private void ConnectSlimes()
    {
        timer.Stop();
        slimesSplit = false;

        Vector3 mergePos = (dashSlime.transform.position + slimeTwo.transform.position) / 2f;
        int newHp = dashSlime.GetComponent<Health>().GetHp() + slimeTwo.GetComponent<Health>().GetHp();

        dashSlime.SetActive(false);
        slimeTwo.SetActive(false);
        CharacterManager.Instance.Spawn("ConnSlimes", mergePos);
        connSlimes.GetComponent<Health>().SetHp(newHp);
    }

    private bool SlimesCanConnect()
    {
        float distance = Vector3.Distance(dashSlime.transform.position, slimeTwo.transform.position);
        return distance <= mergeDistance;
    }

    private void OnTimerEnd()
    {
        if (slimesSplit && SlimesCanConnect())
            ConnectSlimes();
        else
        {
            // level lost condition
            dashSlime.SetActive(false);
            slimeTwo.SetActive(false);
            connSlimes.SetActive(false);
        }
    }
}
