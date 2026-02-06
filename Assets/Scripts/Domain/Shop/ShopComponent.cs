public sealed class ShopComponent
{
    private readonly IWallet wallet;
    private readonly IShopCatalog catalog;

    private readonly ShopProgress progress;
    private readonly IPricePolicy pricePolicy;

    public ShopComponent(IWallet wallet, IShopCatalog catalog, IPricePolicy pricePolicy, ShopProgress progress)
    {
        this.wallet = wallet;
        this.catalog = catalog;
        this.pricePolicy = pricePolicy;
        this.progress = progress;
    }

    public int GetCurrentPrice(string productId)
    {
        if (!catalog.TryGetById(productId, out var product))
            return 0;

        return pricePolicy.GetPrice(product, progress);
    }

    public PurchaseResult TryBuy(string productId, ShopContext context)
    {
        if (string.IsNullOrWhiteSpace(productId))
            return PurchaseResult.Fail("Invalid product id.");

        if (context == null)
            return PurchaseResult.Fail("Missing shop context.");

        if (!catalog.TryGetById(productId, out var product))
            return PurchaseResult.Fail("Product not found.");

        int costNow = pricePolicy.GetPrice(product, progress);

        if (!wallet.CanAfford(costNow))
            return PurchaseResult.Fail("Not enough score.");

        if (!wallet.TrySpend(costNow))
            return PurchaseResult.Fail("Payment failed.");

        product.Apply(context);

        // Count purchases for inflation tracking (can mark all; only upgrades inflate anyway)
        progress.MarkPurchased(product.Id);

        return PurchaseResult.Ok();
    }
}
