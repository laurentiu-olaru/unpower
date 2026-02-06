using System.Collections.Generic;

public sealed class UnityShopCatalog : IShopCatalog
{
    private readonly List<IShopProduct> all = new();
    private readonly Dictionary<string, IShopProduct> byId = new();

    public IReadOnlyList<IShopProduct> All => all;

    public UnityShopCatalog(ShopCatalogSO catalogSo)
    {
        if (catalogSo == null || catalogSo.products == null) return;

        foreach (var so in catalogSo.products)
        {
            if (so == null || string.IsNullOrWhiteSpace(so.id)) continue;

            var product = CreateProduct(so);
            if (product == null) continue;

            all.Add(product);
            byId[product.Id] = product;
        }
    }

    public bool TryGetById(string id, out IShopProduct product)
        => byId.TryGetValue(id, out product);

    private static IShopProduct CreateProduct(ShopProductSO so)
    {
        switch (so.type)
        {
            case ShopProductType.Upgrade:
                return new UpgradeProduct(
                    so.id,
                    so.displayName,
                    so.cost,
					MapUpgradeKind(so.upgradeKind),
                    so.upgradeIntValue,
                    so.upgradeFloatValue
                );

            case ShopProductType.Building:
                if (so.buildingDefinition == null) return null;

                return new BuildingProduct(
                    so.id,
                    so.displayName,
                    so.cost,
                    so.buildingDefinition // <-- this is IBuildingDefinition via interface
                );


            case ShopProductType.Buff:
                return new BuffProduct(so.id, so.displayName, so.cost, so.buffId, so.buffDurationSeconds);
        }

        return null;
    }

	private static UpgradeProduct.UpgradeType MapUpgradeKind(UpgradeKind kind)
	{
		return kind switch
		{
			UpgradeKind.MaxHp => UpgradeProduct.UpgradeType.MaxHp,
			UpgradeKind.Damage => UpgradeProduct.UpgradeType.Damage,
			UpgradeKind.FireRateMultiplier => UpgradeProduct.UpgradeType.FireRateMultiplier,
			UpgradeKind.ArrowDamage => UpgradeProduct.UpgradeType.ArrowDamage,
			UpgradeKind.MoveSpeed => UpgradeProduct.UpgradeType.MoveSpeed,
            UpgradeKind.BarracksUpgrade => UpgradeProduct.UpgradeType.BarracksUpgrade,
            _ => UpgradeProduct.UpgradeType.Damage
		};
	}

}
