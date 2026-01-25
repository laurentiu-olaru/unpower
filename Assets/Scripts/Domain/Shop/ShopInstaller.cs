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
        var shop = new ShopComponent(wallet, domainCatalog);
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


//3rd implementation
//using UnityEngine;

//public class ShopInstaller : MonoBehaviour
//{
//    [Header("Data")]
//    [SerializeField] private ShopCatalogSO catalog;

//    [Header("Currency")]
//    [SerializeField] private PlayerScoreView scoreView;

//    [Header("Placement")]
//    [SerializeField] private PlacementController placementController;

//    [Header("UI")]
//    [SerializeField] private ShopView shopView;

//    private void Awake()
//    {
//        if (catalog == null)
//        {
//            Debug.LogError("[ShopInstaller] Catalog is not assigned.");
//            return;
//        }

//        if (scoreView == null || scoreView.Score == null)
//        {
//            Debug.LogError("[ShopInstaller] ScoreView (or ScoreComponent) is missing.");
//            return;
//        }

//        if (placementController == null)
//        {
//            Debug.LogError("[ShopInstaller] PlacementController is not assigned.");
//            return;
//        }

//        if (shopView == null)
//        {
//            Debug.LogError("[ShopInstaller] ShopView is not assigned.");
//            return;
//        }

//        var wallet = new ScoreWalletAdapter(scoreView.Score);
//        var domainCatalog = new UnityShopCatalog(catalog);
//        var shop = new ShopComponent(wallet, domainCatalog);

//        var ctx = new ShopContext
//        {
//            PlacementRequestor = placementController
//        };

//        shopView.Bind(shop, domainCatalog, ctx);
//    }
//}

//2nd implementation, followed by the first.
//using UnityEngine;

//public class ShopInstaller : MonoBehaviour
//{
//    [Header("Data")]
//    public ShopCatalogSO catalog;

//    [Header("Currency")]
//    public PlayerScoreView scoreView;

//    [Header("Context providers")]
//    public PlayerUpgradesAdapter playerUpgrades;
//    public BuildingSpawnerAdapter buildingSpawner;
//    public BuffApplierAdapter buffApplier;
//    public PlacementController placementController;


//    [Header("UI")]
//    public ShopView shopView;


//    private ShopComponent shop;

//    void Awake()
//    {
//        var wallet = new ScoreWalletAdapter(scoreView.Score);
//        var domainCatalog = new UnityShopCatalog(catalog);

//        shop = new ShopComponent(wallet, domainCatalog);

//        var ctx = new ShopContext
//        {
//            PlayerUpgrades = playerUpgrades,
//            PlacementRequestor = placementController,
//            BuffApplier = buffApplier
//        };

//        shopView.Bind(shop, domainCatalog, ctx);
//    }
//}



/*
 using UnityEngine;

public class ShopInstaller : MonoBehaviour
{
    [Header("Data")]
    public ShopCatalogSO catalog;

    [Header("Currency")]
    public PlayerScoreView scoreView;

    [Header("Placement")]
    public PlacementController placementController;

    [Header("UI")]
    public ShopView shopView;

    void Awake()
    {
        var wallet = new ScoreWalletAdapter(scoreView.Score);
        var domainCatalog = new UnityShopCatalog(catalog);

        var shop = new ShopComponent(wallet, domainCatalog);

        var ctx = new ShopContext
        {
            PlacementRequestor = placementController
        };

        shopView.Bind(shop, domainCatalog, ctx);
    }
}

 */