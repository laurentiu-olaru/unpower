using UnityEngine;

public class ProjectileOwnerView : MonoBehaviour
{
	public GameObject Owner { get; private set; }

	// Layers this projectile is allowed to damage
	[SerializeField] private LayerMask damageableLayers;

	public void SetOwner(GameObject owner) => Owner = owner;

	public bool CanDamage(Collider2D other)
	{
		if (other == null) return false;

		// Don't hit the shooter or shooter children
		if (Owner != null && (other.gameObject == Owner || other.transform.IsChildOf(Owner.transform)))
			return false;

		// Layer filter
		int otherLayerMask = 1 << other.gameObject.layer;
		return (damageableLayers.value & otherLayerMask) != 0;
	}
}
