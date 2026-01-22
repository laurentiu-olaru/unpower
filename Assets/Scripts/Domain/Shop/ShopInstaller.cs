using UnityEngine;

public class ShopInstaller : MonoBehaviour
{
    [Header("Data")]
    public ShopCatalogSO catalog;

    [Header("Currency")]
    public PlayerScoreView scoreView;

    [Header("Context providers")]
    public PlayerUpgradesAdapter playerUpgrades;
    public BuildingSpawnerAdapter buildingSpawner;
    public BuffApplierAdapter buffApplier;
    public PlacementController placementController;


    [Header("UI")]
    public ShopView shopView;

    private ShopComponent shop;

    void Awake()
    {
        var wallet = new ScoreWalletAdapter(scoreView.Score);
        var domainCatalog = new UnityShopCatalog(catalog);

        shop = new ShopComponent(wallet, domainCatalog);

        var ctx = new ShopContext
        {
            PlayerUpgrades = playerUpgrades,
            PlacementRequestor = placementController,
            BuffApplier = buffApplier
        };

        shopView.Bind(shop, domainCatalog, ctx);
    }
}
