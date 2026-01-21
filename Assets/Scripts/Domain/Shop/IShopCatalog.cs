using System.Collections.Generic;

public interface IShopCatalog
{
    IReadOnlyList<IShopProduct> All { get; }
    bool TryGetById(string id, out IShopProduct product);
}
