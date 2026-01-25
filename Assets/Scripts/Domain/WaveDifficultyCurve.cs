using System;

namespace Domain.Waves
{
    public sealed class WaveDifficultyCurve
    {
        // Base values (wave 1)
        private readonly int baseEnemies;
        private readonly float baseBetweenSpawns;
        private readonly float baseBetweenWaves;

        // Scaling per wave
        private readonly int enemiesAddedPerWave;
        private readonly float spawnIntervalMultiplierPerWave; // < 1 means faster
        private readonly float waveRestMultiplierPerWave;      // < 1 means less rest

        // Clamps so it doesn’t get ridiculous
        private readonly float minBetweenSpawns;
        private readonly float minBetweenWaves;

        // Optional stat scaling
        private readonly float hpMultPerWave;
        private readonly float speedMultPerWave;
        private readonly float damageMultPerWave;

        public WaveDifficultyCurve(
            int baseEnemies = 3,
            int enemiesAddedPerWave = 1,
            float baseBetweenSpawns = 1.0f,
            float baseBetweenWaves = 5.0f,
            float spawnIntervalMultiplierPerWave = 0.95f,
            float waveRestMultiplierPerWave = 0.97f,
            float minBetweenSpawns = 0.2f,
            float minBetweenWaves = 1.0f,
            float hpMultPerWave = 1.05f,
            float speedMultPerWave = 1.01f,
            float damageMultPerWave = 1.03f)
        {
            this.baseEnemies = baseEnemies;
            this.enemiesAddedPerWave = enemiesAddedPerWave;
            this.baseBetweenSpawns = baseBetweenSpawns;
            this.baseBetweenWaves = baseBetweenWaves;
            this.spawnIntervalMultiplierPerWave = spawnIntervalMultiplierPerWave;
            this.waveRestMultiplierPerWave = waveRestMultiplierPerWave;
            this.minBetweenSpawns = minBetweenSpawns;
            this.minBetweenWaves = minBetweenWaves;

            this.hpMultPerWave = hpMultPerWave;
            this.speedMultPerWave = speedMultPerWave;
            this.damageMultPerWave = damageMultPerWave;
        }

        public WavePlan GetPlan(WaveIndex wave)
        {
            int w = Math.Max(1, wave.Value);

            int enemies = baseEnemies + (w - 1) * enemiesAddedPerWave;

            float betweenSpawns = ClampMin(
                baseBetweenSpawns * Pow(spawnIntervalMultiplierPerWave, w - 1),
                minBetweenSpawns);

            float betweenWaves = ClampMin(
                baseBetweenWaves * Pow(waveRestMultiplierPerWave, w - 1),
                minBetweenWaves);

            float hp = Pow(hpMultPerWave, w - 1);
            float speed = Pow(speedMultPerWave, w - 1);
            float dmg = Pow(damageMultPerWave, w - 1);

            return new WavePlan(enemies, betweenSpawns, betweenWaves, hp, speed, dmg);
        }

        private static float Pow(float a, int p) => (float)Math.Pow(a, p);
        private static float ClampMin(float v, float min) => v < min ? min : v;
    }
}
