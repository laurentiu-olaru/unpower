namespace Domain.Waves
{
    public readonly struct WaveIndex
    {
        public readonly int Value;
        public WaveIndex(int value) => Value = value < 1 ? 1 : value;
    }
}
