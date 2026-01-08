public class CoinView : PickupView
{
    public int value = 1;

    protected override IPickupEffect CreateEffect()
    {
        return new ScoreEffect(value);
    }
}
