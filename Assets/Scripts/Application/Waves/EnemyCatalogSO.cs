using UnityEngine;

[CreateAssetMenu(menuName = "Waves/Enemy Catalog")]
public class EnemyCatalogSO : ScriptableObject
{
	public EnemyEntry[] enemies;

	public bool TryGetPrefab(string id, out GameObject prefab)
	{
		if (enemies != null)
		{
			for (int i = 0; i < enemies.Length; i++)
			{
				if (enemies[i].id == id && enemies[i].prefab != null)
				{
					prefab = enemies[i].prefab;
					return true;
				}
			}
		}

		prefab = null;
		return false;
	}
}

[System.Serializable]
public class EnemyEntry
{
	public string id;
	public GameObject prefab;
}
