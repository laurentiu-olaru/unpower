using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the Stone Golem's melee attack: detects when the target is in range,
/// plays the attack animation, waits for the hit frame, then applies damage via
/// a Physics2D overlap circle.
///
/// Timeline per attack:
///   0s          — animation trigger fires, movement locked
///   hitTime     — damage circle fires (should match the animation's "impact" frame)
///   lockTime    — movement unlocked, attack cooldown continues
///   cooldown    — next attack allowed (timer starts at attack START, not end)
///
/// Prefab setup checklist:
///   □ Assign AttackPoint child transform (offset in front of the fist/hand)
///   □ Set targetLayers to include Player and Ally physics layers
///   □ Ensure attackRadius >= attackRange (otherwise attacks trigger but miss)
///   □ Confirm the Animator has a trigger parameter matching attackTrigger
/// </summary>
public class BossAttackView : MonoBehaviour
{
	[Header("Dependencies")]
	[SerializeField] private BossTargetFinderView targetFinder;
	[SerializeField] private BossRigidbodyMoverView mover;
	[SerializeField] private Animator animator;

	[Header("Attack Point")]
	[SerializeField] private Transform attackPoint;
	// IMPORTANT: attackRadius must be >= attackRange.
	// attackRange is how close the boss needs to be before it TRIES to attack.
	// attackRadius is the actual damage-circle radius fired from attackPoint.
	// If attackRadius < attackRange the boss can trigger an attack while the
	// target is still outside the damage circle, so hits never register.
	[SerializeField] private float attackRadius = 1.5f;
	[SerializeField] private LayerMask targetLayers; // Player + Ally layers

	[Header("Attack")]
	[SerializeField] private int damage = 25;
	[SerializeField] private float attackRange = 1.4f;     // how close the boss must be before swinging
	[SerializeField] private float attackCooldown = 1.5f;  // seconds between attack STARTS (not between ends)
	[SerializeField] private float hitTime = 0.35f;        // seconds into the animation before damage lands
	[SerializeField] private float lockTime = 0.9f;        // total movement-lock duration per attack

	[Header("Animator")]
	[SerializeField] private string attackTrigger = "Attack";

	private float nextAttackTime;
	private bool isAttacking;

	private void Reset()
	{
		targetFinder = GetComponent<BossTargetFinderView>();
		mover = GetComponent<BossRigidbodyMoverView>();
		animator = GetComponentInChildren<Animator>(true);
		if (attackPoint == null) attackPoint = transform; // fallback
	}

	private void Update()
	{
		// Don't start a new attack while one is already running
		if (isAttacking) return;
		// Cooldown not elapsed yet
		if (Time.time < nextAttackTime) return;

		var target = targetFinder != null ? targetFinder.CurrentTarget : null;
		if (target == null) return;

		// Only attack when the target is within melee range
		float dist = Vector2.Distance(transform.position, target.position);
		if (dist > attackRange) return;

		StartCoroutine(AttackRoutine());
	}

	private IEnumerator AttackRoutine()
	{
		isAttacking = true;

		// Cooldown starts here (at attack start), not at the end of the animation.
		// This means the effective gap between attacks is (attackCooldown - lockTime).
		// Increase attackCooldown if you want more breathing room between swings.
		nextAttackTime = Time.time + attackCooldown;

		// Pin the boss in place so it doesn't slide during the swing
		if (mover != null) mover.SetMovementLocked(true);
		animator?.SetTrigger(attackTrigger);

		// Wait until the animation reaches the "impact" frame before applying damage
		if (hitTime > 0f)
			yield return new WaitForSeconds(hitTime);

		DoDamage();

		// Wait out the rest of the lock time after the hit lands
		float remaining = Mathf.Max(0f, lockTime - hitTime);
		if (remaining > 0f)
			yield return new WaitForSeconds(remaining);

		if (mover != null) mover.SetMovementLocked(false);
		isAttacking = false;
	}

	/// <summary>
	/// Fires a Physics2D overlap circle at the attack point and calls
	/// TakeDamage on every IDamageable found inside it.
	/// </summary>
	private void DoDamage()
	{
		// Fall back to the boss root position if no dedicated attack point is set
		Vector2 center = attackPoint != null ? (Vector2)attackPoint.position : (Vector2)transform.position;

		var hits = Physics2D.OverlapCircleAll(center, attackRadius, targetLayers);
		foreach (var c in hits)
		{
			// GetComponentInParent walks up the hierarchy from the collider's GO.
			// This correctly handles cases where the player/ally Collider2D is on a
			// child object but IDamageable is on the root.
			var damageable = c.GetComponentInParent<IDamageable>();
			if (damageable != null)
				damageable.TakeDamage(damage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 origin = attackPoint != null ? attackPoint.position : transform.position;

		// Red = actual damage circle (what gets hit)
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(origin, attackRadius);

		// Yellow = attack trigger range (how close the boss must be to swing)
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}