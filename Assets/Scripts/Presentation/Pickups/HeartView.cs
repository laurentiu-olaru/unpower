public class HeartView : PickupView
{
    public int healAmount = 30;

    protected override IPickupEffect CreateEffect()
    {
        return new HealEffect(healAmount);
    }
}
