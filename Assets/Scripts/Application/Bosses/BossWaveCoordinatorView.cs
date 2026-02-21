using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaveCoordinatorView : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private BossWaveScheduleSO _schedule;
	[SerializeField] private BossCatalogSO _bossCatalog;
	[SerializeField] private BossDifficultyProfileSO _bossDifficulty;

	[Header("References")]
	[SerializeField] private EnemyDifficultyApplierView _difficultyApplier;
	[SerializeField] private EnemySpawnServiceView _spawnService;

	[Header("Spawn positioning")]
	[SerializeField] private Transform _bossSpawnPoint; // optional; fallback to spawner position if null

	// Optional hook for UI later (you can connect your UI system to this)
	public System.Action<int> OnBossIncoming; // waveNumber
	public System.Action<int> OnBossSpawned;  // waveNumber

	public IEnumerator RunBossSegmentIfAny(int waveNumber)
	{
		if (_schedule == null || !_schedule.TryGetEntry(waveNumber, out var entry))
			yield break;

		// 1) Pre-boss delay (announce)
		OnBossIncoming?.Invoke(waveNumber);
		if (entry.PreBossDelaySeconds > 0f)
			yield return new WaitForSeconds(entry.PreBossDelaySeconds);

		// 2) Spawn bosses
		OnBossSpawned?.Invoke(waveNumber);

		var aliveBosses = new HashSet<EnemyHealthView>();

		if (entry.SpawnSequentially)
		{
			foreach (var bossId in entry.BossIds)
			{
				var health = SpawnSingleBoss(bossId, waveNumber);
				if (health != null) aliveBosses.Add(health);

				if (entry.TimeBetweenBossSpawns > 0f)
					yield return new WaitForSeconds(entry.TimeBetweenBossSpawns);
			}
		}
		else
		{
			foreach (var bossId in entry.BossIds)
			{
				var health = SpawnSingleBoss(bossId, waveNumber);
				if (health != null) aliveBosses.Add(health);
			}
		}

		// 3) Wait until all bosses die
		while (aliveBosses.Count > 0)
			yield return null;

		// Local function for clean closure on death
		EnemyHealthView SpawnSingleBoss(string bossId, int wave)
		{
			if (string.IsNullOrWhiteSpace(bossId))
			{
				Debug.LogWarning("[BossWaveCoordinator] BossId is empty.");
				return null;
			}

			if (_bossCatalog == null || !_bossCatalog.TryGetPrefab(bossId, out var prefab) || prefab == null)
			{
				Debug.LogWarning($"[BossWaveCoordinator] No boss prefab found for id '{bossId}'.");
				return null;
			}

			Vector2 spawnPos = _bossSpawnPoint != null
				? (Vector2)_bossSpawnPoint.position
				: (Vector2)transform.position;

			// Spawn inactive then configure
			GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity);
			go.SetActive(false);

			// Apply boss difficulty
			var (hp, speed, dmg) = _bossDifficulty != null
				? _bossDifficulty.Evaluate(wave)
				: (1f, 1f, 1f);

			if (_difficultyApplier != null)
				_difficultyApplier.Apply(go, hp, speed, dmg);

			//IMPORTANT: include inactive children because GO is inactive right now
			var healthView = go.GetComponentInChildren<EnemyHealthView>(true);
			if (healthView != null)
			{
				// Example: boss drops 10x the normal wave coin count
				int baseCoins = 1 + ((wave - 1) / 5);
				healthView.SetCoinsToDrop(baseCoins * 10);

				healthView.Died += OnBossDied;
			}
			if (healthView == null)
			{
				Debug.LogWarning($"[BossWaveCoordinator] Boss '{bossId}' has no EnemyHealthView (even in inactive children).");
			}
			else
			{
				healthView.Died += OnBossDied;
			}


			go.SetActive(true);
			return healthView;

			void OnBossDied(EnemyHealthView h)
			{
				h.Died -= OnBossDied;
				aliveBosses.Remove(h);
			}
		}
	}
}