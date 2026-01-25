using UnityEngine;

public class EnemySpawnServiceView : MonoBehaviour, IEnemySpawnService
{
    [SerializeField] private GameObject enemyPrefab;

    public GameObject SpawnEnemy(Vector2 position)
    {
        if (enemyPrefab == null) return null;

        // Spawn inactive so nothing runs before we apply difficulty
        var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.SetActive(false);
        return enemy;
    }
}
