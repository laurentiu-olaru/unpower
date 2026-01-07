using UnityEngine;

public class AllyMeleeAI : MonoBehaviour
{
	public float speed = 3f;
	public float attackRange = 1.5f;
	public float attackDamage = 10f;
	public float attackRate = 1f;
	private float nextAttackTime = 0f;

	private Transform target;

	void Update()
	{
		FindNearestEnemy();

		if (target != null)
		{
			float distance = Vector2.Distance(transform.position, target.position);

			if (distance <= attackRange)
			{
				// Attack
				if (Time.time >= nextAttackTime)
				{
					Attack();
					nextAttackTime = Time.time + 1f / attackRate;
				}
			}
			else
			{
				// Move towards enemy
				transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			}
		}
	}

	void FindNearestEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		float shortestDistance = Mathf.Infinity;
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

		if (nearestEnemy != null) target = nearestEnemy.transform;
	}

	void Attack()
	{
		EnemyHealth enemyHP = target.GetComponent<EnemyHealth>();
		if (enemyHP != null)
		{
			enemyHP.TakeDamage(attackDamage);
		}
	}
}