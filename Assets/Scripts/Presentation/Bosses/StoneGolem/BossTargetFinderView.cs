using UnityEngine;

public class BossTargetFinderView : MonoBehaviour
{
	[Header("Targeting")]
	[SerializeField] private float refreshInterval = 0.25f;
	[SerializeField] private string playerTag = "Player";
	[SerializeField] private string allyTag = "Ally";

	public Transform CurrentTarget { get; private set; }

	private float nextRefreshTime;

	private void Update()
	{
		if (Time.time < nextRefreshTime) return;
		nextRefreshTime = Time.time + refreshInterval;

		CurrentTarget = FindNearestTarget();
	}

	private Transform FindNearestTarget()
	{
		Transform nearest = null;
		float bestDist = float.PositiveInfinity;

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

		// Player (single)
		var player = GameObject.FindGameObjectWithTag(playerTag);
		Check(player);

		// Allies (many)
		var allies = GameObject.FindGameObjectsWithTag(allyTag);
		foreach (var ally in allies) Check(ally);

		return nearest;
	}
}