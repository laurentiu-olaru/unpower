using UnityEngine;

public class ProjectileLifetimeView : MonoBehaviour
{
	public void Arm(float lifetime)
	{
		if (lifetime > 0f)
			Destroy(gameObject, lifetime);
	}
}
