using System.Collections;
using UnityEngine;

public class BossAttackView : MonoBehaviour
{
	[Header("Dependencies")]
	[SerializeField] private BossTargetFinderView targetFinder;
	[SerializeField] private BossRigidbodyMoverView mover;
	[SerializeField] private Animator animator;

	[Header("Attack Point")]
	[SerializeField] private Transform attackPoint;
	[SerializeField] private float attackRadius = 1.0f;
	[SerializeField] private LayerMask targetLayers; // Player + Ally layers

	[Header("Attack")]
	[SerializeField] private int damage = 25;
	[SerializeField] private float attackRange = 1.4f;     // when to start attack
	[SerializeField] private float attackCooldown = 1.5f;
	[SerializeField] private float hitTime = 0.35f;        // time until damage moment
	[SerializeField] private float lockTime = 0.9f;        // movement lock duration

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
		if (isAttacking) return;
		if (Time.time < nextAttackTime) return;

		var target = targetFinder != null ? targetFinder.CurrentTarget : null;
		if (target == null) return;

		float dist = Vector2.Distance(transform.position, target.position);
		if (dist > attackRange) return;

		StartCoroutine(AttackRoutine());
	}

	private IEnumerator AttackRoutine()
	{
		isAttacking = true;
		nextAttackTime = Time.time + attackCooldown;

		if (mover != null) mover.SetMovementLocked(true);
		animator?.SetTrigger(attackTrigger);

		if (hitTime > 0f)
			yield return new WaitForSeconds(hitTime);

		DoDamage();

		float remaining = Mathf.Max(0f, lockTime - hitTime);
		if (remaining > 0f)
			yield return new WaitForSeconds(remaining);

		if (mover != null) mover.SetMovementLocked(false);
		isAttacking = false;
	}

	private void DoDamage()
	{
		Vector2 center = attackPoint != null ? (Vector2)attackPoint.position : (Vector2)transform.position;

		var hits = Physics2D.OverlapCircleAll(center, attackRadius, targetLayers);
		foreach (var c in hits)
		{
			// GetComponentInParent is key: handles player colliders on child objects
			var damageable = c.GetComponentInParent<IDamageable>();
			if (damageable != null)
				damageable.TakeDamage(damage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null) return;
		Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
	}
}