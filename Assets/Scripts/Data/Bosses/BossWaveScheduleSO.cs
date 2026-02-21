using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Bosses/Boss Wave Schedule")]
public class BossWaveScheduleSO : ScriptableObject
{
	[Serializable]
	public class BossWaveEntry
	{
		[Min(1)] public int WaveNumber = 20;

		[Tooltip("Wait time after normal wave spawns finish, before bosses spawn.")]
		[Min(0f)] public float PreBossDelaySeconds = 15f;

		[Tooltip("If true, normal spawning is considered 'paused' during boss segment (we do this by design).")]
		public bool PauseNormalSpawning = true;

		[Tooltip("Spawn bosses sequentially (one after another) or all at once.")]
		public bool SpawnSequentially = false;

		[Tooltip("Optional gap between sequential boss spawns.")]
		[Min(0f)] public float TimeBetweenBossSpawns = 0f;

		[Tooltip("Boss IDs to spawn in this boss segment (supports multiple).")]
		public List<string> BossIds = new();
	}

	[SerializeField] private List<BossWaveEntry> _entries = new();

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