public interface IShopProduct
{
    string Id { get; }
    string DisplayName { get; }

    // Base cost from SO (never changes)
    int BaseCost { get; }

    // What kind of product it is (Upgrade/Building/Buff)
    ShopProductType ProductType { get; }

    void Apply(ShopContext context);
}
