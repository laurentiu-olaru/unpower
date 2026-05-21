using System.Collections;
using Domain.Waves;
using UnityEngine;

/// <summary>
/// Orchestrates the entire wave progression loop.
///
/// Each wave:
///   1. Spawns <c>plan.EnemyCount</c> enemies with staggered timing
///   2. Applies per-wave HP/speed/damage multipliers via IEnemyDifficultyApplier
///   3. Hands off to <see cref="BossWaveCoordinatorView"/> for any boss segment
///   4. Waits the rest period, then optionally a longer 5-wave break
///
/// spawnServiceBehaviour and difficultyApplierBehaviour are serialized as
/// MonoBehaviour references (not typed interfaces) so Unity can display them
/// in the Inspector while still binding through the interface at runtime.
/// </summary>
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

    [SerializeField] private WaveHudView hud;

	[SerializeField] private WaveEnemyPoolSO enemyPool;
	[SerializeField] private string fallbackEnemyId = "melee";

	[SerializeField] private BossWaveCoordinatorView _bossCoordinator;


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
			hud?.SetWave(currentWave);

			// 1) Normal enemy spawns for this wave
			for (int i = 0; i < plan.EnemyCount; i++)
			{
				Vector2 pos = GetSpawnPosAroundPlayer();
				string enemyId = fallbackEnemyId;

				if (enemyPool != null && enemyPool.TryPickEnemyId(currentWave, out var picked))
					enemyId = picked;

				var enemy = spawnService.SpawnEnemy(enemyId, pos);

				// +1 coin every 5 waves
				int coinCount = 1 + ((currentWave - 1) / 5);

				if (enemy != null)
				{
					var ehv = enemy.GetComponent<EnemyHealthView>();
					if (ehv != null)
						ehv.SetCoinsToDrop(coinCount);

					if (difficultyApplier != null)
						difficultyApplier.Apply(enemy, plan.EnemyHpMultiplier, plan.EnemySpeedMultiplier, plan.EnemyDamageMultiplier);

					enemy.SetActive(true);
				}

				yield return new WaitForSeconds(plan.TimeBetweenSpawns);
			}

			// 2) Boss segment happens HERE (after normal spawns, before rest)
			if (_bossCoordinator != null)
			{
				yield return _bossCoordinator.RunBossSegmentIfAny(currentWave);
			}

			// 3) Normal rest (same as before)
			yield return new WaitForSeconds(plan.TimeBetweenWaves);

			// 4) Extra break every 5 waves (after wave 5, 10, 15...)
			if (currentWave % 5 == 0)
			{
				hud?.ShowBreak(60);
				yield return new WaitForSeconds(60f);
				hud?.HideBreak();
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
