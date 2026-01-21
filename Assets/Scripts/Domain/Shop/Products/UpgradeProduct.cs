public sealed class UpgradeProduct : IShopProduct
{
    public enum UpgradeType { MaxHp, Damage, FireRateMultiplier }

    public string Id { get; }
    public string DisplayName { get; }
    public int Cost { get; }

    private readonly UpgradeType type;
    private readonly int intValue;
    private readonly float floatValue;

    public UpgradeProduct(string id, string name, int cost, UpgradeType type, int intValue = 0, float floatValue = 0f)
    {
        Id = id;
        DisplayName = name;
        Cost = cost;
        this.type = type;
        this.intValue = intValue;
        this.floatValue = floatValue;
    }

    public void Apply(ShopContext context)
    {
        var upgrades = context.PlayerUpgrades;
        if (upgrades == null) return;

        switch (type)
        {
            case UpgradeType.MaxHp:
                upgrades.AddMaxHp(intValue);
                break;
            case UpgradeType.Damage:
                upgrades.AddDamage(intValue);
                break;
            case UpgradeType.FireRateMultiplier:
                upgrades.AddFireRateMultiplier(floatValue);
                break;
        }
    }
}
