using UnityEngine;

[RequireComponent(typeof(ProjectileMovementView))]
[RequireComponent(typeof(ProjectileLifetimeView))]
[RequireComponent(typeof(DamageOnHitView))]
public class ProjectileView : MonoBehaviour
{
	[SerializeField] private float defaultSpeed = 10f;
	[SerializeField] private float defaultLifetime = 5f;

	private ProjectileMovementView movement;
	private ProjectileLifetimeView lifetime;
	private DamageOnHitView damageOnHit;
	private ProjectileOwnerView owner;

	void Awake()
	{
		movement = GetComponent<ProjectileMovementView>();
		lifetime = GetComponent<ProjectileLifetimeView>();
		damageOnHit = GetComponent<DamageOnHitView>();
		owner = GetComponent<ProjectileOwnerView>();
	}

	public void Configure(ProjectileConfig config, GameObject projectileOwner)
	{
		if (owner != null)
			owner.SetOwner(projectileOwner);

		movement.SetVelocity(config.Direction, config.Speed <= 0 ? defaultSpeed : config.Speed);
		damageOnHit.SetDamage(config.Damage);
		lifetime.Arm(config.Lifetime <= 0 ? defaultLifetime : config.Lifetime);
	}
}
