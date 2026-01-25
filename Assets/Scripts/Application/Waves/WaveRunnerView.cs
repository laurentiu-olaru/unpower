using System.Collections;
using Domain.Waves;
using UnityEngine;

public class WaveRunnerView : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform player;
    [SerializeField] private MonoBehaviour spawnServiceBehaviour; // must implement IEnemySpawnService
    [SerializeField] private MonoBehaviour difficultyApplierBehaviour; // must implement IEnemyDifficultyApplier (optional)

    [Header("Spawn Area")]
    [SerializeField] private float spawnRadius = 8f;

    [Header("Difficulty")]
    [SerializeField] private int startWave = 1;

    private IEnemySpawnService spawnService;
    private IEnemyDifficultyApplier difficultyApplier;

    private WaveDifficultyCurve curve;
    private int currentWave;

    void Awake()
    {
        spawnService = spawnServiceBehaviour as IEnemySpawnService;
        difficultyApplier = difficultyApplierBehaviour as IEnemyDifficultyApplier;

        curve = new WaveDifficultyCurve(); // to-do: make this configurable via ScriptableObject
        currentWave = startWave;

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Start()
    {
        if (spawnService == null)
        {
            Debug.LogError("[WaveRunner] Spawn service is not assigned or doesn't implement IEnemySpawnService.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("[WaveRunner] Player transform not assigned and not found by tag.");
            return;
        }

        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (true)
        {
            var plan = curve.GetPlan(new WaveIndex(currentWave));
            Debug.Log($"[WaveRunner] Wave {currentWave}: enemies={plan.EnemyCount}, spawnGap={plan.TimeBetweenSpawns:0.00}, rest={plan.TimeBetweenWaves:0.00}");

            for (int i = 0; i < plan.EnemyCount; i++)
            {
                Vector2 pos = GetSpawnPosAroundPlayer();
                var enemy = spawnService.SpawnEnemy(pos);

                int coinCount = 1 + ((currentWave - 1) / 5); // +1 coin every 5 waves

                var ehv = enemy.GetComponent<EnemyHealthView>();
                if (ehv != null)
                    ehv.SetCoinsToDrop(coinCount);


                if (enemy != null)
                {
                    if (difficultyApplier != null)
                        difficultyApplier.Apply(enemy, plan.EnemyHpMultiplier, plan.EnemySpeedMultiplier, plan.EnemyDamageMultiplier);

                    // Activate only after difficulty is applied
                    enemy.SetActive(true);
                }

                yield return new WaitForSeconds(plan.TimeBetweenSpawns);

            }


            // normal rest
            yield return new WaitForSeconds(plan.TimeBetweenWaves);

            // extra break every 5 waves (after wave 5, 10, 15...)
            if (currentWave % 5 == 0)
            {
                Debug.Log("[WaveRunner] Break time! 60 seconds.");
                yield return new WaitForSeconds(60f);
            }
            currentWave++;
            
        }
    }

    private Vector2 GetSpawnPosAroundPlayer()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        return (Vector2)player.position + dir * spawnRadius;
    }
}
