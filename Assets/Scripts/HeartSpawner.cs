using UnityEngine;
using System.Collections;

public class HeartSpawner : MonoBehaviour
{
	public GameObject heartPrefab;
	public Transform player;

	[Header("Spawner Settings")]
	public float spawnInterval = 10f;
	public float spawnRadius = 6f;
	public int maxHeartsOnMap = 3;

	void Start()
	{
		if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

		// Start the repeating spawn logic
		StartCoroutine(HeartSpawnRoutine());
	}

	IEnumerator HeartSpawnRoutine()
	{
		while (true) // Keep running throughout the game
		{
			// 1. Find all objects with the "Heart" tag
			GameObject[] existingHearts = GameObject.FindGameObjectsWithTag("Heart");

			// 2. Only spawn if we are under the limit
			if (existingHearts.Length < maxHeartsOnMap)
			{
				SpawnHeart();
			}

			// 3. Wait 10 seconds before checking again
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	void SpawnHeart()
	{
		if (player == null) return;

		// Pick a random spot near the player
		Vector2 randomDir = Random.insideUnitCircle.normalized;
		float randomDist = Random.Range(3f, spawnRadius); // Between 3 and 6 units away
		Vector2 spawnPos = (Vector2)player.position + (randomDir * randomDist);

		Instantiate(heartPrefab, spawnPos, Quaternion.identity);
	}
}