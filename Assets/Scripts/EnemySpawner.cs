using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab;
	public int count = 5;
	public Vector2 spawnMin;
	public Vector2 spawnMax;

	void Start()
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 pos = new Vector2(
				Random.Range(spawnMin.x, spawnMax.x),
				Random.Range(spawnMin.y, spawnMax.y)
			);

			Instantiate(enemyPrefab, pos, Quaternion.identity);
		}
	}
}
