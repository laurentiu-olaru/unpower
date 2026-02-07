using UnityEngine;

public interface IEnemySpawnService
{
	GameObject SpawnEnemy(string enemyId, Vector2 position);
}
