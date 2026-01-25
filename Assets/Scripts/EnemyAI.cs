using UnityEngine;

public class EnemyAI : MonoBehaviour, ITargetable
{
    public float speed = 3f;
    public int damage = 10;
    public float attackSpeed = 1.0f;

    private Transform currentTarget;
    private float lastAttackTime;

	public Transform GetTransform() => transform;

	void Update()
    {
        FindNearestTarget();

        if (currentTarget != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                currentTarget.position,
                speed * Time.deltaTime
            );
        }
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        void Check(GameObject go)
        {
            float d = Vector2.Distance(transform.position, go.transform.position);
            if (d < shortestDistance)
            {
                shortestDistance = d;
                nearest = go.transform;
            }
        }

        if (targets.Length > 0) Check(targets[0]);
        foreach (var ally in allies) Check(ally);

        currentTarget = nearest;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time > lastAttackTime + attackSpeed)
        {

            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null && !collision.gameObject.CompareTag("Enemy"))
            {
                damageable.TakeDamage(damage);
                lastAttackTime = Time.time;
            }

            //var flash = collision.collider.GetComponentInChildren<HitFlash>();
            //flash?.ForceFlash();
        }
    }
}
