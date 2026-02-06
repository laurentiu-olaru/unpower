using System;

public sealed class UpgradeInflationPricePolicy : IPricePolicy
{
    private readonly float upgradeMultiplier;
    private readonly int maxPrice;

    public UpgradeInflationPricePolicy(float upgradeMultiplier = 1.25f, int maxPrice = 9999)
    {
        this.upgradeMultiplier = upgradeMultiplier;
        this.maxPrice = maxPrice;
    }

    public int GetPrice(IShopProduct product, ShopProgress progress)
    {
        // Only upgrades inflate; everything else stays fixed
        if (product.ProductType != ShopProductType.Upgrade)
            return product.BaseCost;

        int bought = progress.GetPurchasedCount(product.Id);

        double scaled = product.BaseCost * Math.Pow(upgradeMultiplier, bought);
        int price = (int)Math.Round(scaled);

        return Math.Min(price, maxPrice);
    }
}
