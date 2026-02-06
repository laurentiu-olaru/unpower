public sealed class UpgradeProduct : IShopProduct
{
    public enum UpgradeType { MaxHp, Damage, FireRateMultiplier, ArrowDamage, MoveSpeed, TowerDamage, BarracksUpgrade }

    public string Id { get; }
    public string DisplayName { get; }

    public int BaseCost { get; }
    public ShopProductType ProductType => ShopProductType.Upgrade;

    private readonly UpgradeType type;
    private readonly int intValue;
    private readonly float floatValue;

    public UpgradeProduct(string id, string name, int baseCost, UpgradeType type, int intValue = 0, float floatValue = 0f)
    {
        Id = id;
        DisplayName = name;
        BaseCost = baseCost;

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
            case UpgradeType.MaxHp: upgrades.AddMaxHp(intValue); break;
            case UpgradeType.Damage: upgrades.AddDamage(intValue); break;
            case UpgradeType.FireRateMultiplier: upgrades.AddFireRateMultiplier(floatValue); break;
            case UpgradeType.ArrowDamage: upgrades.AddArrowDamage(intValue); break;
            case UpgradeType.MoveSpeed: upgrades.AddMoveSpeed(floatValue); break;
            case UpgradeType.TowerDamage: upgrades.AddTowerDamage(intValue); break;
            case UpgradeType.BarracksUpgrade: upgrades.AddBarracksUpgradeLevel(intValue); break;
        }
    }
}
