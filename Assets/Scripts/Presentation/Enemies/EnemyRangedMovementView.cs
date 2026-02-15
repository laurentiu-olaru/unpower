using UnityEngine;

public class EnemyRangedMovementView : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 2.5f;
	[SerializeField] private float preferredRange = 6f;
	[SerializeField] private float stopBuffer = 0.5f;

	[SerializeField] private EnemyMageFacingView facing;


	[Header("Refs")]
	[SerializeField] private EnemyMageAnimatorView anim;

	private ITargetable target;

	void Awake()
	{
		if (facing == null) facing = GetComponent<EnemyMageFacingView>();


		if (anim == null)
			anim = GetComponent<EnemyMageAnimatorView>();
	}

	void Update()
	{
		if (target == null)
			AcquireTarget();

		if (target == null)
		{
			anim?.SetMoving(false);
			return;
		}

		if (!TryGetTargetTransform(out var t))
		{
			AcquireTarget();
			if (!TryGetTargetTransform(out t))
			{
				anim?.SetMoving(false);
				return;
			}
		}


		Vector2 targetPos = t.position;

		float dist = Vector2.Distance(transform.position, targetPos);

		// Too far -> move closer
		if (dist > preferredRange + stopBuffer)
		{
			MoveTowards(targetPos);
			return;
		}

		// Too close -> kite away
		if (dist < preferredRange - stopBuffer)
		{
			MoveAway(targetPos);
			return;
		}

		// In good range -> stop
		anim?.SetMoving(false);
	}

	private void MoveTowards(Vector2 pos)
	{
		anim?.SetMoving(true);
		transform.position = Vector2.MoveTowards(
			transform.position,
			pos,
			moveSpeed * Time.deltaTime
		);
		Vector2 dir = (pos - (Vector2)transform.position).normalized;
		facing?.FaceMoveDirection(dir);

	}

	private void MoveAway(Vector2 pos)
	{
		anim?.SetMoving(true);

		Vector2 dir2D = ((Vector2)transform.position - pos).normalized;
		Vector3 dir3D = new Vector3(dir2D.x, dir2D.y, 0f);

		facing?.FaceMoveDirection(dir2D);

		transform.position += dir3D * moveSpeed * Time.deltaTime;
	}


	private void AcquireTarget()
	{
		// Very simple: reuse same logic as ranged attack
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10f);

		float closest = Mathf.Infinity;
		ITargetable nearest = null;

		foreach (var hit in hits)
		{
			if (hit.TryGetComponent(out ITargetable t))
			{
				if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
					continue;

				float d = Vector2.Distance(transform.position, t.GetTransform().position);
				if (d < closest)
				{
					closest = d;
					nearest = t;
				}
			}
		}

		target = nearest;
	}

	//Scaling hooks 
	public void MultiplySpeed(float multiplier)
	{
		moveSpeed *= multiplier;
	}

	private bool TryGetTargetTransform(out Transform t)
	{
		t = null;
		if (target == null) return false;

		t = target.GetTransform();
		if (t == null)
		{
			target = null;
			return false;
		}

		return true;
	}

}
