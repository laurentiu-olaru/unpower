public sealed class BuildingProduct : IShopProduct
{
    public string Id { get; }
    public string DisplayName { get; }
    public int Cost { get; }

    private readonly string buildingId;

    public BuildingProduct(string id, string name, int cost, string buildingId)
    {
        Id = id;
        DisplayName = name;
        Cost = cost;
        this.buildingId = buildingId;
    }

    public void Apply(ShopContext context)
    {
        context.BuildingSpawner?.Spawn(buildingId);
    }
}
