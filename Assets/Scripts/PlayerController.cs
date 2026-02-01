using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float fireCooldown = 0.15f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private Rigidbody2D rb;
    private PlayerMotor motor;
    private PlayerShooter shooter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        motor = new PlayerMotor(speed);
        shooter = new PlayerShooter(fireCooldown);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && shooter.CanFire(Time.time))
        {
            Shoot();
            shooter.MarkFired(Time.time);
        }
    }

	void FixedUpdate()
	{
		Vector2 input = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		);

		float bonusSpeed = GetComponent<PlayerMoveSpeedModifier>()?.BonusSpeed ?? 0f;

		//temporary motor so upgrades apply immediately
		var runtimeMotor = new PlayerMotor(speed + bonusSpeed);

		rb.linearVelocity = runtimeMotor.ComputeVelocity(input);
	}


	void Shoot()
	{
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.z = 0;

		Vector2 dir = (mouse - firePoint.position).normalized;

		GameObject go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

		var projView = go.GetComponent<ProjectileView>();
		if (projView == null)
		{
			Debug.LogError("[PlayerController] Projectile prefab missing ProjectileView.");
			Destroy(go);
			return;
		}

		int damage = GetComponent<PlayerAttackStatsView>()?.GetProjectileDamage() ?? 10;

		// You can expose these as fields later
		float projSpeed = 10f;
		float lifetime = 5f;

		projView.Configure(new ProjectileConfig(dir, projSpeed, damage, lifetime), gameObject);
	}

}
