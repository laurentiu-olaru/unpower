using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that defines WHICH waves spawn bosses and HOW.
/// Create via: Assets > Create > Game > Bosses > Boss Wave Schedule
///
/// Each entry specifies a wave number and one or more boss IDs to spawn.
/// BossWaveCoordinatorView reads this schedule and handles the actual spawning.
///
/// Example setup for a wave-20 golem boss:
///   WaveNumber         = 20
///   PreBossDelaySeconds = 10    (10-second warning before spawn)
///   SpawnSequentially  = false  (spawn all at once)
///   BossIds            = ["stone_golem"]
/// </summary>
[CreateAssetMenu(menuName = "Game/Bosses/Boss Wave Schedule")]
public class BossWaveScheduleSO : ScriptableObject
{
	/// <summary>All data describing a single boss wave event.</summary>
	[Serializable]
	public class BossWaveEntry
	{
		[Tooltip("Which wave number triggers this boss segment.")]
		[Min(1)] public int WaveNumber = 20;

		[Tooltip("Seconds to wait after normal wave spawns finish before bosses appear. " +
		         "Use this to give players a heads-up (show a UI warning, etc.).")]
		[Min(0f)] public float PreBossDelaySeconds = 15f;

		[Tooltip("Reserved flag — WaveRunnerView already stops normal spawning before " +
		         "calling RunBossSegmentIfAny, so this is informational only.")]
		public bool PauseNormalSpawning = true;

		[Tooltip("If true, each boss in BossIds spawns one after another (with TimeBetweenBossSpawns delay). " +
		         "If false, all bosses spawn simultaneously.")]
		public bool SpawnSequentially = false;

		[Tooltip("Seconds between successive spawns when SpawnSequentially = true. " +
		         "Ignored if SpawnSequentially = false.")]
		[Min(0f)] public float TimeBetweenBossSpawns = 0f;

		[Tooltip("List of boss IDs to spawn (must match entries in BossCatalogSO). " +
		         "Supports multi-boss encounters.")]
		public List<string> BossIds = new();
	}

	[SerializeField] private List<BossWaveEntry> _entries = new();

	/// <summary>
	/// Returns true and outputs the entry if a boss wave is scheduled for
	/// <paramref name="waveNumber"/>. Returns false (and null entry) for normal waves.
	/// </summary>
	public bool TryGetEntry(int waveNumber, out BossWaveEntry entry)
	{
		foreach (var e in _entries)
		{
			if (e.WaveNumber == waveNumber)
			{
				entry = e;
				return true;
			}
		}

		entry = null;
		return false;
	}
}
