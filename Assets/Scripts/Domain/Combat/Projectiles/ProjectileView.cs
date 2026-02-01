using UnityEngine;

[RequireComponent(typeof(ProjectileMovementView))]
[RequireComponent(typeof(ProjectileLifetimeView))]
[RequireComponent(typeof(DamageOnHitView))]
public class ProjectileView : MonoBehaviour
{
	[SerializeField] private float defaultSpeed = 10f;
	[SerializeField] private float defaultLifetime = 5f;

	[Header("Rotation")]
	[SerializeField] private bool rotateToDirection = true;
	[SerializeField] private bool spriteFacesRight = true; // set false if your sprite faces UP by default


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
		if (rotateToDirection)
		{
			Vector2 dir = config.Direction;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			// If sprite faces UP by default, subtract 90 degrees.
			if (!spriteFacesRight)
				angle -= 90f;

			transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}

		damageOnHit.SetDamage(config.Damage);
		lifetime.Arm(config.Lifetime <= 0 ? defaultLifetime : config.Lifetime);
	}
}
