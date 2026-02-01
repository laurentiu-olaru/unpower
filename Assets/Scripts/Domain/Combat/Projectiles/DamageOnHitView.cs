using UnityEngine;

[RequireComponent(typeof(ProjectileHitboxView))]
public class DamageOnHitView : MonoBehaviour
{
	private ProjectileHitboxView hitbox;
	private ProjectileOwnerView owner;
	private int damage;

	void Awake()
	{
		hitbox = GetComponent<ProjectileHitboxView>();
		owner = GetComponent<ProjectileOwnerView>();
	}

	void OnEnable()
	{
		hitbox.Hit += OnHit;
	}

	void OnDisable()
	{
		hitbox.Hit -= OnHit;
	}

	public void SetDamage(int value)
	{
		damage = Mathf.Max(0, value);
	}

	private void OnHit(Collider2D other)
	{
		if (other == null) return;

		if (owner != null && !owner.CanDamage(other))
			return;

		if (other.TryGetComponent(out IDamageable dmg))
		{
			dmg.TakeDamage(damage);
			Destroy(gameObject);
		}
	}
}
