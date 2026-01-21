using System;

public class ScoreComponent
{
    public int Score { get; private set; }

    public event Action<int> OnScoreChanged;

    public void Add(int amount)
    {
        if (amount <= 0) return;
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public bool CanAfford(int amount) => amount >= 0 && Score >= amount;

    public bool TrySpend(int amount)
    {
        if (amount <= 0) return false;
        if (Score < amount) return false;

        Score -= amount;
        OnScoreChanged?.Invoke(Score);
        return true;
    }
}
