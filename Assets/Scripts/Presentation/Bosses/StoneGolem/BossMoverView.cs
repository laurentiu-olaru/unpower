using UnityEngine;

public class BossMoverView : MonoBehaviour
{
	[SerializeField] private BossTargetFinderView targetFinder;
	[SerializeField] private float moveSpeed = 2.0f;
	[SerializeField] private float stopDistance = 1.2f;

	private void Reset()
	{
		targetFinder = GetComponent<BossTargetFinderView>();
	}

	private void Update()
	{
		if (targetFinder == null) return;
		var target = targetFinder.CurrentTarget;
		if (target == null) return;

		Vector2 pos = transform.position;
		Vector2 tpos = target.position;

		float dist = Vector2.Distance(pos, tpos);
		if (dist <= stopDistance) return;

		Vector2 next = Vector2.MoveTowards(pos, tpos, moveSpeed * Time.deltaTime);
		transform.position = next;
	}
}