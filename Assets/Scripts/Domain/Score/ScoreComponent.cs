using System;

public class ScoreComponent
{
    public int Score { get; private set; }

    public event Action<int> OnScoreChanged;


    public void Initialize(int startingScore)
	{
		// Previously hardcoded to 10, ignoring the startingScore parameter entirely.
		// Every game session started with 10 coins regardless of what was passed in.
		Score = startingScore;
		OnScoreChanged?.Invoke(Score);
	}

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
