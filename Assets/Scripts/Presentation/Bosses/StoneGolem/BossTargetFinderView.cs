using UnityEngine;

/// <summary>
/// Periodically scans the scene to find the nearest valid target (Player or Ally)
/// and exposes it as <see cref="CurrentTarget"/> for other boss components to read.
///
/// Design note: All boss components (mover, attacker, facing) poll CurrentTarget
/// rather than each doing their own scene search, so the expensive
/// FindGameObjectsWithTag call only runs once every <see cref="refreshInterval"/>
/// seconds instead of every frame per-component.
/// </summary>
public class BossTargetFinderView : MonoBehaviour
{
	[Header("Targeting")]
	/// <summary>Seconds between target refresh passes. Lower = more responsive but more expensive.</summary>
	[SerializeField] private float refreshInterval = 0.25f;
	[SerializeField] private string playerTag = "Player";
	[SerializeField] private string allyTag = "Ally";

	/// <summary>The nearest living Player or Ally transform. May be null if none exist in the scene.</summary>
	public Transform CurrentTarget { get; private set; }

	private float nextRefreshTime;

	private void Update()
	{
		// Rate-limit the search to avoid calling FindGameObjectsWithTag every frame
		if (Time.time < nextRefreshTime) return;
		nextRefreshTime = Time.time + refreshInterval;

		CurrentTarget = FindNearestTarget();
	}

	/// <summary>
	/// Scans all Player-tagged and Ally-tagged objects and returns the Transform
	/// closest to this boss. Returns null if no valid targets exist.
	/// </summary>
	private Transform FindNearestTarget()
	{
		Transform nearest = null;
		float bestDist = float.PositiveInfinity;

		// Local helper: updates nearest/bestDist if this object is closer
		void Check(GameObject go)
		{
			if (go == null) return;
			float d = Vector2.Distance(transform.position, go.transform.position);
			if (d < bestDist)
			{
				bestDist = d;
				nearest = go.transform;
			}
		}

		// Check the single player object
		var player = GameObject.FindGameObjectWithTag(playerTag);
		Check(player);

		// Check all allies (barracks units, etc.)
		var allies = GameObject.FindGameObjectsWithTag(allyTag);
		foreach (var ally in allies) Check(ally);

		return nearest;
	}
}
