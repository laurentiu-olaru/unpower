using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public float speed = 3f;
	private Transform player;
	public int damage = 10;
	public float attackSpeed = 1.0f; // Seconds between hits
	private float lastAttackTime;



	void Start()
	{
		// Find the player by tag
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null) player = playerObj.transform;
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null) player = playerObj.transform;
		if (collision.gameObject.CompareTag("Player"))
		{
			if (Time.time > lastAttackTime + attackSpeed)
			{
				Health ph = collision.gameObject.GetComponent<Health>();
				if (ph != null)
				{
					ph.TakeDamage(damage);
					lastAttackTime = Time.time;
					Debug.Log("Enemy Attacked Player!");
				}
			}
		}
	}

	void Update()
	{
		if (player != null)
		{
			// Move towards the player
			transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
		}
	}
}