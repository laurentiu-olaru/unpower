using System.Collections.Generic;

public sealed class ShopProgress
{
    private readonly Dictionary<string, int> purchasedCounts = new();

    public int GetPurchasedCount(string productId)
        => purchasedCounts.TryGetValue(productId, out var c) ? c : 0;

    public void MarkPurchased(string productId)
    {
        purchasedCounts[productId] = GetPurchasedCount(productId) + 1;
    }
}
