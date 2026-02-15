using UnityEngine;

public class TargetableView : MonoBehaviour, ITargetable
{
	public Transform GetTransform()
	{
		// Unity's destroyed-object null check:
		if (this == null) return null;
		return transform;
	}
}
