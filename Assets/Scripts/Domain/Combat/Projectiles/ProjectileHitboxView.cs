using System;
using UnityEngine;

public class ProjectileHitboxView : MonoBehaviour
{
	public event Action<Collider2D> Hit;

	private void OnTriggerEnter2D(Collider2D other)
	{
		Hit?.Invoke(other);
	}
}
