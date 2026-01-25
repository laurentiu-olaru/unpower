namespace Domain.Waves
{
    public readonly struct WavePlan
    {
        public readonly int EnemyCount;
        public readonly float TimeBetweenSpawns;
        public readonly float TimeBetweenWaves;

        // Optional difficulty knobs
        public readonly float EnemyHpMultiplier;
        public readonly float EnemySpeedMultiplier;
        public readonly float EnemyDamageMultiplier;

        public WavePlan(
            int enemyCount,
            float timeBetweenSpawns,
            float timeBetweenWaves,
            float hpMult = 1f,
            float speedMult = 1f,
            float damageMult = 1f)
        {
            EnemyCount = enemyCount;
            TimeBetweenSpawns = timeBetweenSpawns;
            TimeBetweenWaves = timeBetweenWaves;
            EnemyHpMultiplier = hpMult;
            EnemySpeedMultiplier = speedMult;
            EnemyDamageMultiplier = damageMult;
        }
    }
}
