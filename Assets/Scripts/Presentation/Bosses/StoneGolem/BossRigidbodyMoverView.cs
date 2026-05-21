using UnityEngine;

/// <summary>
/// Moves the boss toward its current target using Rigidbody2D velocity.
///
/// Velocity-based movement (via FixedUpdate + rb.linearVelocity) is preferred over
/// directly writing transform.position because:
/// - Collisions are resolved correctly by the physics engine each step
/// - There is no tunnelling through thin colliders
/// - OnCollisionEnter2D / OnTriggerEnter2D callbacks fire reliably
///
/// Movement can be locked externally by <see cref="BossAttackView"/> during
/// the attack animation so the boss stays planted while swinging.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BossRigidbodyMoverView : MonoBehaviour
{
	[SerializeField] private BossTargetFinderView targetFinder;

	/// <summary>World-units per second the boss moves toward its target.</summary>
	[SerializeField] private float moveSpeed = 2f;

	/// <summary>
	/// The boss stops moving when it is within this distance of the target.
	/// Should roughly match <see cref="BossAttackView.attackRange"/> so the
	/// boss stops just as it enters melee range.
	/// </summary>
	[SerializeField] private float stopDistance = 1.2f;

	private Rigidbody2D rb;

	/// <summary>
	/// When true, the mover zeroes velocity every FixedUpdate and ignores the target.
	/// Used by BossAttackView to pin the boss in place during an attack swing.
	/// </summary>
	private bool movementLocked;

	/// <summary>Locks or unlocks movement. Called by BossAttackView at attack start/end.</summary>
	public void SetMovementLocked(bool locked) => movementLocked = locked;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Reset()
	{
		// Auto-wire in the Inspector when the component is first added
		targetFinder = GetComponent<BossTargetFinderView>();
	}

	private void FixedUpdate()
	{
		// If movement is locked (e.g. mid-attack), keep the boss perfectly still
		if (movementLocked)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}

		if (targetFinder == null) return;

		var target = targetFinder.CurrentTarget;
		if (target == null)
		{
			// No target — coast to a stop
			rb.linearVelocity = Vector2.zero;
			return;
		}

		Vector2 pos = rb.position;
		Vector2 tpos = target.position;

		float dist = Vector2.Distance(pos, tpos);

		// Already close enough — stop and wait for BossAttackView to swing
		if (dist <= stopDistance)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}

		// Move at constant speed toward the target
		Vector2 dir = (tpos - pos).normalized;
		rb.linearVelocity = dir * moveSpeed;
	}
}
