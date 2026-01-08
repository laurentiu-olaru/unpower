public class HealEffect : IPickupEffect
{
    private readonly int amount;

    public HealEffect(int amount)
    {
        this.amount = amount;
    }

    public void Apply(PickupContext context)
    {
        context.Health?.Heal(amount);
    }
}
