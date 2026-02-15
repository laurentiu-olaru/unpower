using UnityEngine;

public class EnemyRangedAttackView : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float shootRange = 7f;

    [Header("Attack")]
    [SerializeField] private float fireCooldown = 1.2f;
    [SerializeField] private Transform firePoint;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileLifetime = 4f;
    [SerializeField] private int projectileDamage = 5;



	[SerializeField] private EnemyMageFacingView facing;

	[SerializeField] private EnemyMageAnimatorView anim;

	private float nextFireTime;
    private ITargetable target;

    void Update()
    {
        AcquireTarget();

        if (target == null) return;

        Transform t = target.GetTransform();
        float dist = Vector2.Distance(transform.position, t.position);

        if (dist > shootRange) return;

        if (Time.time >= nextFireTime)
        {
            ShootAt(t.position);
            nextFireTime = Time.time + fireCooldown;
        }
    }
	private void Awake()
	{
		if (facing == null) facing = GetComponent<EnemyMageFacingView>();

		if (anim == null) anim = GetComponent<EnemyMageAnimatorView>();

	}

	private void AcquireTarget()
    {
		// Keep current target if still valid and within detection
		if (target != null)
		{
			var t = target.GetTransform();
			if (t == null)
			{
				target = null;
			}
			else if (Vector2.Distance(transform.position, t.position) <= detectionRange)
			{
				return;
			}
			else
			{
				target = null;
			}
		}


		target = FindNearestTargetable(detectionRange);
    }

    private ITargetable FindNearestTargetable(float range)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        float closest = Mathf.Infinity;
        ITargetable nearest = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ITargetable t))
            {
                // IMPORTANT: don't target other enemies (by layer)
                if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    continue;

                float d = Vector2.Distance(transform.position, t.GetTransform().position);
                if (d < closest)
                {
                    closest = d;
                    nearest = t;
                }
            }
        }

        return nearest;
    }

    private void ShootAt(Vector3 targetPos)
    {
        if (projectilePrefab == null || firePoint == null) return;

        Vector2 dir = (targetPos - firePoint.position).normalized;
		facing?.FaceTargetDirection(dir);

		anim?.PlayAttack();

		GameObject go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        var pv = go.GetComponent<ProjectileView>();
        if (pv == null)
        {
            Debug.LogError("[EnemyRangedAttackView] Projectile prefab missing ProjectileView.");
            Destroy(go);
            return;
        }

        pv.Configure(new ProjectileConfig(dir, projectileSpeed, projectileDamage, projectileLifetime), gameObject);
    }

	public void MultiplyDamage(float multiplier)
	{
		projectileDamage = Mathf.RoundToInt(projectileDamage * multiplier);
	}

	public void MultiplyFireRate(float multiplier)
	{
		// higher multiplier = faster firing
		fireCooldown = Mathf.Max(0.1f, fireCooldown / multiplier);
	}

}
