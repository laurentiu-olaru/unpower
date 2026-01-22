using System;
using UnityEngine;

public class BuildingSpawnerAdapter : MonoBehaviour, IBuildingSpawner
{
    [Serializable]
    public struct BuildingEntry
    {
        public string buildingId;
        public GameObject prefab;
    }

    public BuildingEntry[] buildings;

    public void Spawn(string buildingId)
    {
        foreach (var b in buildings)
        {
            if (b.buildingId == buildingId && b.prefab != null)
            {
                Instantiate(b.prefab, transform.position, Quaternion.identity);
                return;
            }
        }

        Debug.LogWarning($"Building id not found: {buildingId}");
    }
}
