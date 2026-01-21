public sealed class ShopComponent
{
    private readonly IWallet wallet;
    private readonly IShopCatalog catalog;

    public ShopComponent(IWallet wallet, IShopCatalog catalog)
    {
        this.wallet = wallet;
        this.catalog = catalog;
    }

    public PurchaseResult TryBuy(string productId, ShopContext context)
    {
        if (string.IsNullOrWhiteSpace(productId))
            return PurchaseResult.Fail("Invalid product id.");

        if (context == null)
            return PurchaseResult.Fail("Missing shop context.");

        if (!catalog.TryGetById(productId, out var product))
            return PurchaseResult.Fail("Product not found.");

        if (!wallet.CanAfford(product.Cost))
            return PurchaseResult.Fail("Not enough score.");

        if (!wallet.TrySpend(product.Cost))
            return PurchaseResult.Fail("Payment failed.");

        product.Apply(context);
        return PurchaseResult.Ok();
    }
}
