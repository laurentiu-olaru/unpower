using UnityEngine;

public interface IEnemyDifficultyApplier
{
    void Apply(GameObject enemyInstance, float hpMult, float speedMult, float damageMult);
}
