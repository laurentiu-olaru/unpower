using UnityEngine;

public class EnemySpawnServiceView : MonoBehaviour, IEnemySpawnService
{
	[SerializeField] private EnemyCatalogSO catalog;

	public GameObject SpawnEnemy(string enemyId, Vector2 position)
	{
		if (catalog == null)
		{
			Debug.LogError("[EnemySpawnService] Missing EnemyCatalogSO.");
			return null;
		}

		if (!catalog.TryGetPrefab(enemyId, out var prefab) || prefab == null)
		{
			Debug.LogError($"[EnemySpawnService] Unknown enemyId '{enemyId}'. Check catalog.");
			return null;
		}

		var enemy = Instantiate(prefab, position, Quaternion.identity);
		enemy.SetActive(false);
		return enemy;
	}
}
