using UnityEngine;

/// <summary>
/// Basic melee enemy AI. Moves toward the nearest Player or Ally each frame
/// and deals contact damage via OnCollisionStay2D.
///
/// Limitations / known issues:
///   - FindNearestTarget runs every Update() via FindGameObjectsWithTag, which
///     is an O(n) scene search. Fine for small enemy counts; consider caching or
///     switching to a spatial query for large waves.
///   - Only the first Player object (targets[0]) is considered. In a single-player
///     game this is fine, but note the inconsistency with allies (all are checked).
///   - Movement uses transform.position directly. If a Rigidbody2D is attached,
///     prefer velocity-based movement in FixedUpdate (see BossRigidbodyMoverView
///     for the correct pattern).
/// </summary>
public class EnemyAI : MonoBehaviour, ITargetable
{
    /// <summary>World-units per second this enemy moves toward its target.</summary>
    public float speed = 3f;

    /// <summary>Damage dealt per hit when colliding with a damageable object.</summary>
    public int damage = 10;

    /// <summary>
    /// Minimum seconds between successive damage applications on the same collision.
    /// Lower values = faster attack rate. Named "attackSpeed" but acts as a cooldown.
    /// </summary>
    public float attackSpeed = 1.0f;

    private Transform currentTarget;
    private float lastAttackTime;

    /// <summary>Returns this enemy's Transform (satisfies ITargetable).</summary>
    public Transform GetTransform() => transform;

    void Update()
    {
        // Re-evaluate the nearest target every frame
        FindNearestTarget();

        if (currentTarget != null)
        {
            // Step toward the target at a constant speed, independent of frame rate
            transform.position = Vector2.MoveTowards(
                transform.position,
                currentTarget.position,
                speed * Time.deltaTime
            );
        }
    }

    /// <summary>
    /// Scans all Player and Ally tagged objects and stores the closest one
    /// in <see cref="currentTarget"/>. Called every Update frame.
    /// </summary>
    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] allies  = GameObject.FindGameObjectsWithTag("Ally");

        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        // Local helper: updates nearest if this object is closer than the current best
        void Check(GameObject go)
        {
            float d = Vector2.Distance(transform.position, go.transform.position);
            if (d < shortestDistance)
            {
                shortestDistance = d;
                nearest = go.transform;
            }
        }

        // Only check the first player found (single-player assumption).
        // All allies are checked so the enemy will target whichever ally/player is closest.
        if (targets.Length > 0) Check(targets[0]);
        foreach (var ally in allies) Check(ally);

        currentTarget = nearest;
    }

    /// <summary>
    /// Called by Unity's physics system every frame this enemy stays in contact
    /// with another collider. Deals damage to any IDamageable that isn't tagged Enemy
    /// (prevents enemies from hurting each other).
    /// </summary>
    void OnCollisionStay2D(Collision2D collision)
    {
        // Rate-limit damage so one continuous collision doesn't drain HP instantly
        if (Time.time > lastAttackTime + attackSpeed)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            // Don't damage other enemies (friendly fire guard)
            if (damageable != null && !collision.gameObject.CompareTag("Enemy"))
            {
                damageable.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }
}
