using UnityEngine;

public class EnemyMageAnimatorView : MonoBehaviour
{
	[SerializeField] private Animator animator;

	// Animator parameter names (keep as constants so typos don’t kill you)
	private static readonly int IsMoving = Animator.StringToHash("IsMoving");
	private static readonly int Attack = Animator.StringToHash("Attack");

	void Awake()
	{
		if (animator == null)
			animator = GetComponentInChildren<Animator>();
	}

	public void SetMoving(bool moving)
	{
		if (animator != null)
			animator.SetBool(IsMoving, moving);
	}

	public void PlayAttack()
	{
		if (animator != null)
			animator.SetTrigger(Attack);
	}
}
