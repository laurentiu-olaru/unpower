using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barracks : MonoBehaviour
{
	public GameObject allyPrefab;
	public Transform spawnPoint; // Create an empty child object on the prefab for this
	public int maxAllies = 3;
	public float spawnInterval = 120f; // 2 minutes in seconds

	private List<GameObject> activeAllies = new List<GameObject>();
	private bool isPlaced = false;

	// This is called by your PlacementManager when the building is built
	public void InitializeBarracks()
	{
		isPlaced = true;
		// Spawn the first set immediately
		SpawnWave();
		// Start the 2-minute timer
		StartCoroutine(SpawnRoutine());
	}

	IEnumerator SpawnRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnInterval);

			if (isPlaced)
			{
				SpawnWave();
			}
		}
	}

	void SpawnWave()
	{
		// Clean up any "null" entries in the list (allies that died)
		activeAllies.RemoveAll(item => item == null);

		// Check how many we can spawn without exceeding the limit
		int spaceLeft = maxAllies - activeAllies.Count;

		for (int i = 0; i < spaceLeft; i++)
		{
			SpawnAlly();
		}
	}

	void SpawnAlly()
	{
		GameObject newAlly = Instantiate(allyPrefab, spawnPoint.position, Quaternion.identity);

		// Pass the Barracks transform to the ally so it knows where "home" is
		AllyMeleeAI ai = newAlly.GetComponent<AllyMeleeAI>();
		if (ai != null)
		{
			ai.homeBase = this.transform;
		}

		activeAllies.Add(newAlly);
	}
}