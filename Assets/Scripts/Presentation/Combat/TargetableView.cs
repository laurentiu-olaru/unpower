using UnityEngine;

public class TargetableView : MonoBehaviour, ITargetable
{
	public Transform GetTransform() => transform;
}
