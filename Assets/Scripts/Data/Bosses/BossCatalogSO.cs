using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject registry that maps string boss IDs to prefabs.
/// Create via: Assets > Create > Game > Bosses > Boss Catalog
///
/// Usage:
///   1. Add entries in the Inspector (BossId = "stone_golem", Prefab = your prefab)
///   2. Reference this SO from BossWaveCoordinatorView
///   3. BossWaveScheduleSO entries reference bosses by their string IDs
///
/// The internal dictionary is built in OnEnable (called when the asset is loaded),
/// so lookups at runtime are O(1) rather than a list search.
/// </summary>
[CreateAssetMenu(menuName = "Game/Bosses/Boss Catalog")]
public class BossCatalogSO : ScriptableObject
{
	/// <summary>A single catalog entry pairing a string ID with a prefab.</summary>
	[Serializable]
	public class Entry
	{
		[Tooltip("Unique string identifier for this boss. Must match the ID used in BossWaveScheduleSO.")]
		public string BossId;

		[Tooltip("The prefab to instantiate when this boss is spawned.")]
		public GameObject Prefab;
	}

	[SerializeField] private List<Entry> _entries = new();

	/// <summary>Populated from _entries on asset load. Key = BossId, Value = prefab.</summary>
	private Dictionary<string, GameObject> _map;

	private void OnEnable()
	{
		// Build the lookup dictionary when the asset is loaded into memory.
		// This runs in the editor (on asset import) and in-game (on first access).
		_map = new Dictionary<string, GameObject>(StringComparer.Ordinal);
		foreach (var e in _entries)
		{
			if (string.IsNullOrWhiteSpace(e.BossId) || e.Prefab == null)
				continue;

			_map[e.BossId] = e.Prefab;
		}
	}

	/// <summary>
	/// Returns true and outputs the prefab if a boss with <paramref name="bossId"/> exists.
	/// Safe to call at runtime — lazily rebuilds the map if it's somehow null.
	/// </summary>
	public bool TryGetPrefab(string bossId, out GameObject prefab)
	{
		if (_map == null) OnEnable(); // defensive: rebuild if map was cleared
		return _map.TryGetValue(bossId, out prefab);
	}
}
