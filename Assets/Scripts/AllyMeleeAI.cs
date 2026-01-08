using UnityEngine;

public class AllyMeleeAI : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 1.5f;
    public float detectionRange = 10f;
    public int attackDamage = 10;
    public float attackRate = 1f;

    [HideInInspector] public Transform homeBase;

    private ITargetable target;
    private float nextAttackTime;

    void Update()
    {
        FindNearestTarget();

        if (target != null)
            MoveToAndAttackTarget();
        else if (homeBase != null)
            ReturnHome();
    }

    void FindNearestTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        float closest = Mathf.Infinity;
        ITargetable nearest = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ITargetable t))
            {
                float d = Vector2.Distance(transform.position, t.GetTransform().position);
                if (d < closest)
                {
                    closest = d;
                    nearest = t;
                }
            }
        }

        target = nearest;
    }

    void MoveToAndAttackTarget()
    {
        Transform t = target.GetTransform();
        float distance = Vector2.Distance(transform.position, t.position);

        if (distance <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack(t);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, t.position, speed * Time.deltaTime);
        }
    }

    void Attack(Transform t)
    {
        if (t.TryGetComponent(out IDamageable dmg))
            dmg.TakeDamage(attackDamage);
    }

    void ReturnHome()
    {
        float d = Vector2.Distance(transform.position, homeBase.position);
        if (d > 2f)
            transform.position = Vector2.MoveTowards(transform.position, homeBase.position, speed * Time.deltaTime);
    }
}
