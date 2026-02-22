using UnityEngine;

public class ColliderDebugView : MonoBehaviour
{
	private Collider2D col;

	private void Awake()
	{
		col = GetComponent<Collider2D>();
	}

	private void OnDrawGizmos()
	{
		if (col == null) col = GetComponent<Collider2D>();
		if (col == null) return;

		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
	}
}