using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
	public GameObject projectilePrefab;
	public float range = 5f;
	public float fireRate = 1f; // One shot per second
	private float fireCountdown = 0f;

	[HideInInspector] // This hides it in the Inspector to keep it clean
	public bool isPlaced = false;

	void Update()
	{
		if(!isPlaced) return; // This fixes the tower attacking while in placement menu 
		fireCountdown -= Time.deltaTime;

		// Find the nearest enemy
		GameObject nearestEnemy = FindNearestEnemy();

		if (nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= range)
		{
			if (fireCountdown <= 0f)
			{
				Shoot(nearestEnemy.transform);
				fireCountdown = 1f / fireRate;
			}
		}
	}

	GameObject FindNearestEnemy()
	{
		// This assumes your enemies are tagged "Enemy"
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject nearest = null;
		float minDistance = Mathf.Infinity;

		foreach (GameObject enemy in enemies)
		{
			float dist = Vector2.Distance(transform.position, enemy.transform.position);
			if (dist < minDistance)
			{
				nearest = enemy;
				minDistance = dist;
			}
		}
		return nearest;
	}

	void Shoot(Transform target)
	{
		GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

		// If your projectile has a "Seek" function, you can pass the target
		// For now, we'll just give it a direction
		Vector2 direction = (target.position - transform.position).normalized;
		proj.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f; // Speed of 10
	}
}