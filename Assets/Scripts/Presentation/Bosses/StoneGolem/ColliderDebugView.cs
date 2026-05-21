using UnityEngine;

public class ColliderDebugView : MonoBehaviour
{
	private Collider2D col;

	private void Awake()
	{
		col = GetComponent<Collider2D>();
	}

	// OnDrawGizmosSelected only draws when this GameObject is selected in
	// the Scene view. Using OnDrawGizmos (always-on) caused visual noise
	// for every object with this component, regardless of selection.
	private void OnDrawGizmosSelected()
	{
		if (col == null) col = GetComponent<Collider2D>();
		if (col == null) return;

		Gizmos.color = Color.cyan;
		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
	}
}