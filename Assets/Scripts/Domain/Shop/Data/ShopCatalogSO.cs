using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Catalog")]
public class ShopCatalogSO : ScriptableObject
{
    public ShopProductSO[] products;
}
