public class ScoreEffect : IPickupEffect
{
    private readonly int amount;

    public ScoreEffect(int amount)
    {
        this.amount = amount;
    }

    public void Apply(PickupContext context)
    {
        context.Score?.Add(amount);
    }
}
