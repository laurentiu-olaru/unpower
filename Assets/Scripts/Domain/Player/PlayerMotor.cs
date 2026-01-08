using UnityEngine;
public class PlayerMotor
{
    public float Speed { get; }

    public PlayerMotor(float speed)
    {
        Speed = speed;
    }

    public Vector2 ComputeVelocity(Vector2 input)
    {
        if (input.magnitude > 1f)
            input = input.normalized;

        return input * Speed;
    }
}
