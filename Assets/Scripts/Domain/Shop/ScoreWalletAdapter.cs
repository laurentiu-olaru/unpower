public sealed class ScoreWalletAdapter : IWallet
{
    private readonly ScoreComponent score;

    public ScoreWalletAdapter(ScoreComponent score)
    {
        this.score = score;
    }

    public int Balance => score.Score;

    public bool CanAfford(int amount) => score.CanAfford(amount);

    public bool TrySpend(int amount) => score.TrySpend(amount);
}
