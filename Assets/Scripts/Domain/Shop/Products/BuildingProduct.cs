public sealed class BuildingProduct : IShopProduct
{
    public string Id { get; }
    public string DisplayName { get; }
    public int Cost { get; }

    private readonly IBuildingDefinition definition;

    public BuildingProduct(string id, string name, int cost, IBuildingDefinition definition)
    {
        Id = id;
        DisplayName = name;
        Cost = cost;
        this.definition = definition;
    }

    public void Apply(ShopContext context)
    {
        context.PlacementRequestor?.BeginPlacement(definition);
    }
}
