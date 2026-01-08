public class PlayerShooter
{

    public float FireCooldown { get; }
    private float lastFireTime = -999f;

    public PlayerShooter(float cooldown)
    {
        FireCooldown = cooldown;
    }

    public bool CanFire(float time)
    {
        return time - lastFireTime >= FireCooldown;
    }

    public void MarkFired(float time)
    {
        lastFireTime = time;
    }


}
