using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab;
	public Transform player;

	[Header("Wave Settings")]
	public int totalEnemiesToSpawn = 15;
	public int enemiesPerWave = 3;
	public float timeBetweenWaveSpawns = 1.0f; // Time between the 3 enemies
	public float timeBetweenWaves = 5.0f;      // Time to rest between waves
	public float spawnRadius = 8.0f;

	private int currentTotalSpawned = 0;

	void Start()
	{
		if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

		// Start the wave logic
		StartCoroutine(SpawnWaves());
	}

	IEnumerator SpawnWaves()
	{
		while (currentTotalSpawned < totalEnemiesToSpawn)
		{
			// Spawn one wave (3 enemies)
			for (int i = 0; i < enemiesPerWave; i++)
			{
				if (currentTotalSpawned < totalEnemiesToSpawn)
				{
					SpawnEnemy();
					currentTotalSpawned++;
					// Wait a tiny bit between each of the 3 enemies so they don't stack
					yield return new WaitForSeconds(timeBetweenWaveSpawns);
				}
			}

			// Wait for the next wave
			Debug.Log("Wave complete! Waiting for next wave...");
			yield return new WaitForSeconds(timeBetweenWaves);
		}

		Debug.Log("All 15 enemies spawned!");
	}

	void SpawnEnemy()
	{
		if (player == null) return;

		// Pick a random spot inside a circle around the player
		Vector2 randomDir = Random.insideUnitCircle.normalized;
		Vector2 spawnPos = (Vector2)player.position + (randomDir * spawnRadius);

		Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
	}
}