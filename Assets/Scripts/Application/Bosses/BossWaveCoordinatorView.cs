using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plugs into <see cref="WaveRunnerView"/> to add boss segments at regular wave intervals.
///
/// Spawn trigger (checked in this order):
///   1. If BossWaveScheduleSO has an explicit entry for this wave → use that entry's
///      settings (delay, boss IDs, sequential timing, etc.).
///   2. Else if the wave number is a multiple of <see cref="_bossIntervalWaves"/> and
///      <see cref="_intervalBossId"/> is set → spawn that boss with default settings.
///   3. Otherwise → skip silently, no boss this wave.
///
/// After all bosses die, control returns to WaveRunnerView which runs the normal
/// inter-wave rest period (TimeBetweenWaves) exactly as it does for every other wave.
/// No extra delay is added here.
/// </summary>
public class BossWaveCoordinatorView : MonoBehaviour
{
	[Header("ScriptableObject Data")]
	[Tooltip("Optional — explicit per-wave boss schedule. Takes priority over interval spawning.")]
	[SerializeField] private BossWaveScheduleSO _schedule;
	[SerializeField] private BossCatalogSO _bossCatalog;
	[SerializeField] private BossDifficultyProfileSO _bossDifficulty;

	[Header("Interval-Based Spawning")]
	[Tooltip("Spawn a boss automatically every N waves (e.g. 20 = waves 20, 40, 60…). " +
	         "Set to 0 to disable interval spawning and rely solely on the schedule above.")]
	[Min(0)] [SerializeField] private int _bossIntervalWaves = 20;

	[Tooltip("The boss ID (from BossCatalogSO) to spawn on interval waves. " +
	         "Must match an entry in the catalog. Leave empty to disable interval spawning.")]
	[SerializeField] private string _intervalBossId = "stone_golem";

	[Header("References")]
	[SerializeField] private EnemyDifficultyApplierView _difficultyApplier;
	[SerializeField] private EnemySpawnServiceView _spawnService;

	[Header("Spawn Positioning")]
	[Tooltip("Where the boss appears. Falls back to this GameObject's position if unassigned.")]
	[SerializeField] private Transform _bossSpawnPoint;

	/// <summary>Fired just before the pre-boss delay begins. Hook this up to show a UI warning.</summary>
	public System.Action<int> OnBossIncoming; // arg: waveNumber

	/// <summary>Fired once the boss(es) actually start spawning.</summary>
	public System.Action<int> OnBossSpawned;  // arg: waveNumber

	/// <summary>
	/// Called by WaveRunnerView each wave. Spawns a boss segment if one is scheduled or
	/// the interval fires, then yields until all bosses are dead before returning.
	/// </summary>
	public IEnumerator RunBossSegmentIfAny(int waveNumber)
	{
		// --- Decide whether to run a boss segment this wave ---
		// Declared separately so the compiler knows it's always initialised.
		// With 'out var' inside a short-circuit &&, if _schedule is null the right
		// side never runs and the compiler considers the variable unassigned (CS0165).
		BossWaveScheduleSO.BossWaveEntry scheduleEntry = null;
		bool hasScheduleEntry = _schedule != null && _schedule.TryGetEntry(waveNumber, out scheduleEntry);

		bool isIntervalWave = _bossIntervalWaves > 0
		                   && waveNumber % _bossIntervalWaves == 0
		                   && !string.IsNullOrWhiteSpace(_intervalBossId);

		// Nothing to do — return immediately with zero overhead
		if (!hasScheduleEntry && !isIntervalWave)
			yield break;

		// --- Pre-boss announcement delay ---
		OnBossIncoming?.Invoke(waveNumber);

		float preDelay = hasScheduleEntry ? scheduleEntry.PreBossDelaySeconds : 0f;
		if (preDelay > 0f)
			yield return new WaitForSeconds(preDelay);

		// --- Spawn the boss(es) ---
		OnBossSpawned?.Invoke(waveNumber);

		var aliveBosses = new HashSet<EnemyHealthView>();

		if (hasScheduleEntry)
		{
			// Use the explicit schedule entry — supports multiple bosses, sequential spawns, etc.
			if (scheduleEntry.SpawnSequentially)
			{
				foreach (var bossId in scheduleEntry.BossIds)
				{
					var health = SpawnSingleBoss(bossId, waveNumber);
					if (health != null) aliveBosses.Add(health);

					if (scheduleEntry.TimeBetweenBossSpawns > 0f)
						yield return new WaitForSeconds(scheduleEntry.TimeBetweenBossSpawns);
				}
			}
			else
			{
				// Spawn all at once
				foreach (var bossId in scheduleEntry.BossIds)
				{
					var health = SpawnSingleBoss(bossId, waveNumber);
					if (health != null) aliveBosses.Add(health);
				}
			}
		}
		else
		{
			// Interval wave — spawn the single default boss immediately
			var health = SpawnSingleBoss(_intervalBossId, waveNumber);
			if (health != null) aliveBosses.Add(health);
		}

		// --- Wait until every spawned boss is dead ---
		// aliveBosses shrinks as each boss fires its Died event (see OnBossDied below).
		// WaveRunnerView is blocked here until the set empties, then it runs the
		// normal inter-wave rest period automatically.
		while (aliveBosses.Count > 0)
			yield return null;

		// ----------------------------------------------------------------
		// Local helper — spawns one boss by ID, wires up the death callback,
		// and applies difficulty scaling. Returns null on any failure.
		// ----------------------------------------------------------------
		EnemyHealthView SpawnSingleBoss(string bossId, int wave)
		{
			if (string.IsNullOrWhiteSpace(bossId))
			{
				Debug.LogWarning("[BossWaveCoordinator] BossId is empty — skipping spawn.");
				return null;
			}

			if (_bossCatalog == null || !_bossCatalog.TryGetPrefab(bossId, out var prefab) || prefab == null)
			{
				Debug.LogWarning($"[BossWaveCoordinator] No prefab found for boss id '{bossId}'. " +
				                 "Check that the id matches an entry in BossCatalogSO.");
				return null;
			}

			Vector2 spawnPos = _bossSpawnPoint != null
				? (Vector2)_bossSpawnPoint.position
				: (Vector2)transform.position;

			// Spawn inactive first so we can configure the GO before any scripts run
			GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity);
			go.SetActive(false);

			// Apply difficulty scaling (HP / speed / damage multipliers)
			var (hp, speed, dmg) = _bossDifficulty != null
				? _bossDifficulty.Evaluate(wave)
				: (1f, 1f, 1f);

			if (_difficultyApplier != null)
				_difficultyApplier.Apply(go, hp, speed, dmg);

			// Search inactive children too because the GO is still inactive here
			var healthView = go.GetComponentInChildren<EnemyHealthView>(true);
			if (healthView == null)
			{
				// Without a health view we can never detect death, so the while loop
				// above would block the wave runner forever.
				Debug.LogWarning($"[BossWaveCoordinator] Boss '{bossId}' has no EnemyHealthView " +
				                 "(checked inactive children too). Boss will block the wave runner!");
			}
			else
			{
				// Reward the player with 10× the normal wave coin count for killing a boss
				int baseCoins = 1 + ((wave - 1) / 5);
				healthView.SetCoinsToDrop(baseCoins * 10);

				// Subscribe ONCE — multiple subscriptions would call OnBossDied multiple
				// times per death and leave dangling delegates.
				healthView.Died += OnBossDied;
			}

			go.SetActive(true);
			return healthView;

			// Nested local function so it closes over 'aliveBosses' by reference
			void OnBossDied(EnemyHealthView h)
			{
				h.Died -= OnBossDied; // clean up the subscription immediately
				aliveBosses.Remove(h);
			}
		}
	}
}
