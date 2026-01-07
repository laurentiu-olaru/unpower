using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public float speed = 3f;
	private Transform currentTarget; // We renamed 'player' to 'currentTarget'
	public int damage = 10;
	public float attackSpeed = 1.0f;
	private float lastAttackTime;

	void Update()
	{
		// Constantly look for the closest target (Player or Ally)
		FindNearestTarget();

		if (currentTarget != null)
		{
			// Move towards the current target
			transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
		}
	}

	void FindNearestTarget()
	{
		// 1. Find the Player
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		// 2. Find all Allies
		GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

		float shortestDistance = Mathf.Infinity;
		Transform nearest = null;

		// Check distance to Player
		if (player != null)
		{
			float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
			if (distToPlayer < shortestDistance)
			{
				shortestDistance = distToPlayer;
				nearest = player.transform;
			}
		}

		// Check distance to all Allies
		foreach (GameObject ally in allies)
		{
			float distToAlly = Vector2.Distance(transform.position, ally.transform.position);
			if (distToAlly < shortestDistance)
			{
				shortestDistance = distToAlly;
				nearest = ally.transform;
			}
		}

		currentTarget = nearest;
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		// Check if enough time has passed to attack
		if (Time.time > lastAttackTime + attackSpeed)
		{
			// CASE 1: We hit the Player
			if (collision.gameObject.CompareTag("Player"))
			{
				// Note: Ensure your Player script is named "Health" or "PlayerHealth" and matches here
				Health playerHP = collision.gameObject.GetComponent<Health>();
				if (playerHP != null)
				{
					playerHP.TakeDamage(damage);
					lastAttackTime = Time.time;
					Debug.Log("Enemy Attacked Player!");
				}
			}
			// CASE 2: We hit an Ally
			else if (collision.gameObject.CompareTag("Ally"))
			{
				AllyHealth allyHP = collision.gameObject.GetComponent<AllyHealth>();
				if (allyHP != null)
				{
					allyHP.TakeDamage(damage);
					lastAttackTime = Time.time;
					Debug.Log("Enemy Attacked Ally!");
				}
			}
		}
	}
}