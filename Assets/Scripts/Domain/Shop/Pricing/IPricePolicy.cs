public interface IPricePolicy
{
    int GetPrice(IShopProduct product, ShopProgress progress);
}
