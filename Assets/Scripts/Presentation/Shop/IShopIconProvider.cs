using UnityEngine;

public interface IShopIconProvider
{
	bool TryGetIcon(string productId, out Sprite icon);
}
