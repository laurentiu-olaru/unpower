using UnityEngine;

public class EnemyDifficultyApplierView : MonoBehaviour, IEnemyDifficultyApplier
{
    public void Apply(GameObject enemyInstance, float hpMult, float speedMult, float damageMult)
    {
        if (enemyInstance == null) return;

        // HP (works with EnemyHealthView now)
        if (enemyInstance.TryGetComponent(out IHealthInitializable initHp))
        {
            // EnemyHealthView has maxHP field; scale it before Start builds HealthComponent
            if (enemyInstance.TryGetComponent(out EnemyHealthView view))
            {
                int scaledHp = Mathf.RoundToInt(view.maxHP * hpMult);
                initHp.SetMaxHp(scaledHp);
            }
        }

        // Movement / damage (EnemyAI)
        if (enemyInstance.TryGetComponent(out EnemyAI ai))
        {
            ai.speed *= speedMult;
            ai.damage = Mathf.RoundToInt(ai.damage * damageMult);
        }
    }
}
