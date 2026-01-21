public interface IShopProduct
{
    string Id { get; }
    string DisplayName { get; }
    int Cost { get; }

    void Apply(ShopContext context);
}
