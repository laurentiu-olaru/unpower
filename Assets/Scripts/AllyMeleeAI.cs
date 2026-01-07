using UnityEngine;

public class AllyMeleeAI : MonoBehaviour
{
	public float speed = 3f;
	public float attackRange = 1.5f;
	public float detectionRange = 10f; // Range to spot enemies
	public float attackDamage = 10f;
	public float attackRate = 1f;

	[HideInInspector] public Transform homeBase; // Set by the Barracks
	private Transform target;
	private float nextAttackTime = 0f;

	void Update()
	{
		FindNearestEnemy();

		if (target != null)
		{
			MoveToAndAttackTarget();
		}
		else if (homeBase != null)
		{
			ReturnHome();
		}
	}

	void MoveToAndAttackTarget()
	{
		float distance = Vector2.Distance(transform.position, target.position);

		if (distance <= attackRange)
		{
			if (Time.time >= nextAttackTime)
			{
				Attack();
				nextAttackTime = Time.time + 1f / attackRate;
			}
		}
		else
		{
			transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		}
	}

	void ReturnHome()
	{
		// Stop a little bit away from the building so they don't all stack on one point
		float distToHome = Vector2.Distance(transform.position, homeBase.position);
		if (distToHome > 2f)
		{
			transform.position = Vector2.MoveTowards(transform.position, homeBase.position, speed * Time.deltaTime);
		}
	}

	void FindNearestEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		float shortestDistance = detectionRange; // Only find enemies within detection range
		GameObject nearestEnemy = null;

		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		target = (nearestEnemy != null) ? nearestEnemy.transform : null;
	}

	void Attack()
	{
		EnemyHealth enemyHP = target.GetComponent<EnemyHealth>();
		if (enemyHP != null) enemyHP.TakeDamage(attackDamage);
	}
}