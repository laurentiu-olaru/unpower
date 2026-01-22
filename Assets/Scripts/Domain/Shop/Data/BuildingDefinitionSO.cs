using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Definition")]
public class BuildingDefinitionSO : ScriptableObject, IBuildingDefinition
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;

    [Header("Placement/Prefab")]
    public GameObject prefab;

    // Add whatever your PlacementManager already uses:
    // footprint size, snap settings, blocking layers, costs, etc.

    public string Id => id;
    public string DisplayName => displayName;
}
