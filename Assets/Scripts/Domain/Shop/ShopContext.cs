public sealed class ShopContext
{
    public IPlayerUpgrades PlayerUpgrades;
    public IBuffApplier BuffApplier;

    // Replaces IBuildingSpawner for your use-case:
    public IPlacementRequestor PlacementRequestor;
}
