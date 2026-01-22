using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopItemRowView : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text costText;
    public Button buyButton;

    private IShopProduct product;
    private Action<IShopProduct> onBuy;

    public void Bind(IShopProduct product, Action<IShopProduct> onBuy)
    {
        this.product = product;
        this.onBuy = onBuy;

        if (nameText != null) nameText.text = product.DisplayName;
        if (costText != null) costText.text = product.Cost.ToString();

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => this.onBuy?.Invoke(this.product));
        }
    }
}
