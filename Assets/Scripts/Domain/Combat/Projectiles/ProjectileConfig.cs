using UnityEngine;

public readonly struct ProjectileConfig
{
	public readonly Vector2 Direction;
	public readonly float Speed;
	public readonly int Damage;
	public readonly float Lifetime;

	public ProjectileConfig(Vector2 dir, float speed, int damage, float lifetime)
	{
		Direction = dir.normalized;
		Speed = speed;
		Damage = damage;
		Lifetime = lifetime;
	}
}
