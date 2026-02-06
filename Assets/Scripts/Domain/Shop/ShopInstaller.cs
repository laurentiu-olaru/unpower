using System.Collections;
using UnityEngine;

public class ShopInstaller : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ShopCatalogSO catalog;

    [Header("Currency")]
    [SerializeField] private PlayerScoreView scoreView;

    [Header("Placement")]
    [SerializeField] private PlacementController placementController;

    [Header("UI")]
    [SerializeField] private ShopView shopView;

    [Header("Upgrades")]
    [SerializeField] private PlayerUpgradesAdapter playerUpgrades;

    private IEnumerator Start()
    {
        // Wait one frame so PlayerScoreView.Awake definitely runs first
        yield return null;

        if (catalog == null)
        {
            Debug.LogError("[ShopInstaller] Catalog is not assigned.");
            yield break;
        }

        if (scoreView == null)
        {
            Debug.LogError("[ShopInstaller] ScoreView is not assigned.");
            yield break;
        }

        if (scoreView.Score == null)
        {
            Debug.LogError("[ShopInstaller] ScoreComponent is still null. PlayerScoreView didn't initialize it.");
            yield break;
        }

        if (placementController == null)
        {
            Debug.LogError("[ShopInstaller] PlacementController is not assigned.");
            yield break;
        }

        if (shopView == null)
        {
            Debug.LogError("[ShopInstaller] ShopView is not assigned.");
            yield break;
        }

        var wallet = new ScoreWalletAdapter(scoreView.Score);
        var domainCatalog = new UnityShopCatalog(catalog);

        var progress = new ShopProgress();
        var pricing = new UpgradeInflationPricePolicy(1.25f);

        var shop = new ShopComponent(wallet, domainCatalog, pricing, progress);

        var iconProvider = new ShopCatalogIconProvider(catalog);

        var ctx = new ShopContext
        {
            PlacementRequestor = placementController,
            PlayerUpgrades = playerUpgrades
        };

        shopView.Bind(shop, domainCatalog, ctx, iconProvider);

        Debug.Log($"[ShopInstaller] Wired OK. Products: {domainCatalog.All.Count}");
    }
}
