using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossRigidbodyMoverView : MonoBehaviour
{
	[SerializeField] private BossTargetFinderView targetFinder;
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float stopDistance = 1.2f;

	private Rigidbody2D rb;
	private bool movementLocked;

	public void SetMovementLocked(bool locked) => movementLocked = locked;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Reset()
	{
		targetFinder = GetComponent<BossTargetFinderView>();
	}

	private void FixedUpdate()
	{
		if (movementLocked)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}

		if (targetFinder == null) return;
		var target = targetFinder.CurrentTarget;
		if (target == null)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}

		Vector2 pos = rb.position;
		Vector2 tpos = target.position;

		float dist = Vector2.Distance(pos, tpos);
		if (dist <= stopDistance)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}

		Vector2 dir = (tpos - pos).normalized;
		rb.linearVelocity = dir * moveSpeed;
	}
}