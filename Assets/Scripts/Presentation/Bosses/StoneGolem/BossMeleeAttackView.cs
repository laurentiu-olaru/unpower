using UnityEngine;

public class BossMeleeAttackView : MonoBehaviour
{
	[Header("Attack")]
	[SerializeField] private int damage = 25;
	[SerializeField] private float attackCooldown = 1.25f;

	[Header("Filtering")]
	[SerializeField] private string enemyTag = "Enemy"; // so we don't damage enemies by accident

	[Header("Animation (optional)")]
	[SerializeField] private Animator animator;
	[SerializeField] private string attackTriggerName = "Attack";

	private float nextAttackTime;

	private void Reset()
	{
		animator = GetComponentInChildren<Animator>();
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (Time.time < nextAttackTime) return;

		// Avoid friendly fire against enemies
		if (collision.gameObject.CompareTag(enemyTag)) return;

		var damageable = collision.gameObject.GetComponent<IDamageable>();
		if (damageable == null) return;

		// Attack
		animator?.SetTrigger(attackTriggerName);
		damageable.TakeDamage(damage);

		nextAttackTime = Time.time + attackCooldown;
	}
}