using UnityEngine;

[CreateAssetMenu(menuName = "Waves/Wave Enemy Pool")]
public class WaveEnemyPoolSO : ScriptableObject
{
	public PoolEntry[] pool;

	public bool TryPickEnemyId(int currentWave, out string enemyId)
	{
		enemyId = null;
		if (pool == null || pool.Length == 0) return false;

		float total = 0f;
		for (int i = 0; i < pool.Length; i++)
		{
			if (currentWave >= pool[i].minWave && pool[i].weight > 0f && !string.IsNullOrEmpty(pool[i].enemyId))
				total += pool[i].weight;
		}

		if (total <= 0f) return false;

		float r = Random.value * total;
		float acc = 0f;

		for (int i = 0; i < pool.Length; i++)
		{
			if (currentWave < pool[i].minWave) continue;
			if (pool[i].weight <= 0f) continue;
			if (string.IsNullOrEmpty(pool[i].enemyId)) continue;

			acc += pool[i].weight;
			if (r <= acc)
			{
				enemyId = pool[i].enemyId;
				return true;
			}
		}

		// fallback
		enemyId = pool[0].enemyId;
		return true;
	}
}

[System.Serializable]
public class PoolEntry
{
	public string enemyId;
	[Range(0f, 10f)] public float weight = 1f;
	public int minWave = 1;
}
