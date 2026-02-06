public sealed class BuffProduct : IShopProduct
{
    public string Id { get; }
    public string DisplayName { get; }
    public int BaseCost { get; }
    public ShopProductType ProductType => ShopProductType.Buff;

    private readonly string buffId;
    private readonly float durationSeconds;

    public BuffProduct(string id, string name, int cost, string buffId, float durationSeconds)
    {
        Id = id;
        DisplayName = name;
        BaseCost = cost;
        this.buffId = buffId;
        this.durationSeconds = durationSeconds;
    }

    public void Apply(ShopContext context)
    {
        context.BuffApplier?.Apply(buffId, durationSeconds);
    }
}
