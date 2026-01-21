public interface IWallet
{
    int Balance { get; }
    bool CanAfford(int amount);
    bool TrySpend(int amount);
}
