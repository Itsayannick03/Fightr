public class Timer
{
    int SecondsLeft;

    public Timer()
    {
        this.SecondsLeft = 300;
    }

    private int GetMinutes()
    {
        return this.SecondsLeft / 60;
    }

    private int GetSeconds()
    {
        return this.SecondsLeft % 60;
    }

    public void ReduceTime(int ammountInSeconds)
    {
        this.SecondsLeft -= ammountInSeconds;

        if(this.SecondsLeft < 0)
            this.SecondsLeft = 0;
    }

    public int GetRemaining()
    {
        return this.SecondsLeft;
    }

    public bool IsTimeOut()
    {
        if(this.SecondsLeft == 0)
            return true;
        else
            return false;
    }

    public void PrintTime()
    {
        int seconds = this.GetSeconds();
        int minutes = this.GetMinutes();

        Console.WriteLine($"{minutes}:{seconds:D2}");
    }
}