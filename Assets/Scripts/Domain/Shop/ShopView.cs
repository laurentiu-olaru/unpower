using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [Header("UI")]
    public Transform listRoot;
    public ShopItemRowView rowPrefab;
    public TMP_Text feedbackText;

    private ShopComponent shop;
    private IShopCatalog catalog;
    private ShopContext context;

    public void Bind(ShopComponent shop, IShopCatalog catalog, ShopContext context)
    {
        this.shop = shop;
        this.catalog = catalog;
        this.context = context;

        Rebuild();
    }

    private void Rebuild()
    {
        if (listRoot == null || rowPrefab == null || catalog == null) return;

        for (int i = listRoot.childCount - 1; i >= 0; i--)
            Destroy(listRoot.GetChild(i).gameObject);

        foreach (var product in catalog.All)
        {
            var row = Instantiate(rowPrefab, listRoot);
            row.Bind(product, OnBuyClicked);
        }
    }

    private void OnBuyClicked(IShopProduct product)
    {
        var result = shop.TryBuy(product.Id, context);

        if (feedbackText != null)
            feedbackText.text = result.Success ? "Purchased!" : result.Reason;
    }
}
