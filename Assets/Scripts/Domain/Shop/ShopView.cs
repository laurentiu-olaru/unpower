using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

public class ShopView : MonoBehaviour
{
    [Header("UI")]
    public Transform listRoot;
    public ShopItemRowView rowPrefab;
    public TMP_Text feedbackText;

	[Header("Feedback")]
	[SerializeField] private string defaultHeaderText = "Build Menu";
	[SerializeField] private float feedbackSeconds = 1.5f;

	// NEW: lets another Unity script react (e.g., close build menu)
	public event Action OnPurchaseSucceeded;

    private ShopComponent shop;
    private IShopCatalog catalog;
    private ShopContext context;
	private IShopIconProvider iconProvider;


	private Coroutine feedbackRoutine;

	public void Bind(ShopComponent shop, IShopCatalog catalog, ShopContext context, IShopIconProvider iconProvider)
    {
        this.shop = shop;
        this.catalog = catalog;
        this.context = context;
		this.iconProvider = iconProvider;

		SetHeader(defaultHeaderText);

		Rebuild();
    }

    private void Rebuild()
    {
        if (listRoot == null || rowPrefab == null || catalog == null) return;

        for (int i = listRoot.childCount - 1; i >= 0; i--)
            Destroy(listRoot.GetChild(i).gameObject);

        foreach (var product in catalog.All)
        {
			Sprite icon = null;
			iconProvider?.TryGetIcon(product.Id, out icon);
			var row = Instantiate(rowPrefab, listRoot);
            row.Bind(product, OnBuyClicked, icon);
        }
    }

	private void SetHeader(string text)
	{
		if (feedbackText != null)
			feedbackText.text = text;
	}

	private IEnumerator FeedbackRoutine(string message)
	{
		SetHeader(message);
		yield return new WaitForSeconds(feedbackSeconds);
		SetHeader(defaultHeaderText);
		feedbackRoutine = null;
	}

	private void ShowTemporaryMessage(string message)
	{
		if (feedbackText == null) return;

		if (feedbackRoutine != null)
			StopCoroutine(feedbackRoutine);

		feedbackRoutine = StartCoroutine(FeedbackRoutine(message));
	}

	private void OnBuyClicked(IShopProduct product)
    {
        if (shop == null || context == null)
        {
            if (feedbackText != null) feedbackText.text = "Shop not wired.";
            return;
        }

        var result = shop.TryBuy(product.Id, context);

        if (feedbackText != null)
        {
			ShowTemporaryMessage("Purchased! Place it...");
			OnPurchaseSucceeded?.Invoke();
		}
        else
        {
			ShowTemporaryMessage(result.Reason);
		}
            

        if (result.Success)
            OnPurchaseSucceeded?.Invoke();
    }

}
