public class Timer
{
    private float duration;
    private float timeRemaining;
    private bool isRunning;

    public bool IsRunning => isRunning;
    public float Remaining => timeRemaining;

    public Timer(float duration)
    {
        this.duration = duration;
        this.timeRemaining = duration;
        this.isRunning = false;
    }

    public void Update(float deltaTime)
    {
        if (!isRunning) return;

        timeRemaining -= deltaTime;
        if (timeRemaining <= 0f)
        {
            isRunning = false;
            timeRemaining = 0f;
        }
    }

    public bool IsFinished()
    {
        if (timeRemaining <= 0f)
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        timeRemaining = duration;
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }
}
