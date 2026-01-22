using UnityEngine;

public enum ShopProductType
{
    Upgrade,
    Building,
    Buff
}

[CreateAssetMenu(menuName = "Shop/Product")]
public class ShopProductSO : ScriptableObject
{
    public string id;
    public string displayName;
    public int cost;

    public ShopProductType type;

    [Header("Upgrade")]
    public UpgradeKind upgradeKind;
    public int upgradeIntValue;
    public float upgradeFloatValue;

    [Header("Building")]
    public BuildingDefinitionSO buildingDefinition;


    [Header("Buff")]
    public string buffId;
    public float buffDurationSeconds = 5f;
}

public enum UpgradeKind
{
    MaxHp,
    Damage,
    FireRateMultiplier
}
