using System;
public class ScoreComponent
{
    public int Score { get; private set; }

    public event Action<int> OnScoreChanged;

    public void Add(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }
}
