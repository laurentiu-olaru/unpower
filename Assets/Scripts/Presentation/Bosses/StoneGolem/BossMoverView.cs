using UnityEngine;

// -----------------------------------------------------------------------
// LEGACY / KINEMATIC MOVER — use BossRigidbodyMoverView instead.
//
// This script moves the boss by writing directly to transform.position.
// That works fine on a plain GameObject, but if a Rigidbody2D is also
// attached Unity will teleport the physics body each frame instead of
// integrating it smoothly through the physics engine, causing jitter,
// tunnelling through thin colliders, and incorrect collision callbacks.
//
// The Stone Golem prefab uses a Rigidbody2D, so BossRigidbodyMoverView
// (which drives movement via rb.linearVelocity in FixedUpdate) is the
// correct mover to use on that prefab. Keep this component disabled or
// remove it from the prefab entirely.
// -----------------------------------------------------------------------
public class BossMoverView : MonoBehaviour
{
	[SerializeField] private BossTargetFinderView targetFinder;
	[SerializeField] private float moveSpeed = 2.0f;
	[SerializeField] private float stopDistance = 1.2f;

	private void Reset()
	{
		targetFinder = GetComponent<BossTargetFinderView>();
	}

	private void Awake()
	{
		// Warn at runtime if both movers are active on the same object
		if (GetComponent<Rigidbody2D>() != null && GetComponent<BossRigidbodyMoverView>() != null)
		{
			Debug.LogWarning(
				$"[BossMoverView] '{name}' has both BossMoverView AND BossRigidbodyMoverView active. " +
				"BossMoverView writes transform.position directly which conflicts with the Rigidbody2D. " +
				"Disable BossMoverView and use BossRigidbodyMoverView instead.",
				this);
		}
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