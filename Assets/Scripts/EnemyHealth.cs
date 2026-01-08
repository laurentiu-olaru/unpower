using UnityEngine;
using UnityEngine.Events;
public class EnemyHealth : MonoBehaviour, IDamageable, ITargetable
{
    public Transform GetTransform() => transform;
    public int maxHealth = 100;
    private int currentHealth;

    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        onDamageTaken?.Invoke();

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            Die();
        }
    }
    void Die()
    {
        GetComponent<EnemyDropper>()?.Drop();
        Destroy(gameObject);
    }
}
   