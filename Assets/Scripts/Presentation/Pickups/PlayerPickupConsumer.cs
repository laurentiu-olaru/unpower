using UnityEngine;

public class PlayerPickupConsumer : MonoBehaviour
{
    public HealthView healthView;
    public PlayerScoreView scoreView;

    public PickupContext Context { get; private set; }

    void Start()
    {
        Context = new PickupContext
        {
            Health = healthView.Health,
            Score = scoreView.Score
        };
    }
}
