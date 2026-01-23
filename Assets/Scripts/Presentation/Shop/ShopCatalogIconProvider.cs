using UnityEngine;

public class ShopCatalogIconProvider : IShopIconProvider
{
	private readonly ShopCatalogSO catalog;

	public ShopCatalogIconProvider(ShopCatalogSO catalog)
	{
		this.catalog = catalog;
	}

	public bool TryGetIcon(string productId, out Sprite icon)
	{
		icon = null;
		if (catalog == null || catalog.products == null) return false;

		foreach (var p in catalog.products)
		{
			if (p != null && p.id == productId)
			{
				icon = p.icon;
				return icon != null;
			}
		}

		return false;
	}
}
