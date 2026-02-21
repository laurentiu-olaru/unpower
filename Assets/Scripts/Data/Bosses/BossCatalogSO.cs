using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Bosses/Boss Catalog")]
public class BossCatalogSO : ScriptableObject
{
	[Serializable]
	public class Entry
	{
		public string BossId;
		public GameObject Prefab;
	}

	[SerializeField] private List<Entry> _entries = new();

	private Dictionary<string, GameObject> _map;

	private void OnEnable()
	{
		_map = new Dictionary<string, GameObject>(StringComparer.Ordinal);
		foreach (var e in _entries)
		{
			if (string.IsNullOrWhiteSpace(e.BossId) || e.Prefab == null)
				continue;

			_map[e.BossId] = e.Prefab;
		}
	}

	public bool TryGetPrefab(string bossId, out GameObject prefab)
	{
		if (_map == null) OnEnable();
		return _map.TryGetValue(bossId, out prefab);
	}
}