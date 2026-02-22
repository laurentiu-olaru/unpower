using UnityEngine;

public class BossFacingView : MonoBehaviour
{
	[Header("Dependencies")]
	[SerializeField] private BossTargetFinderView targetFinder;

	[Header("Visual to flip")]
	[SerializeField] private Transform visualRoot; // the Visual child
	[SerializeField] private SpriteRenderer spriteRenderer; // optional if you prefer flipX

	[Header("Attack point mirroring")]
	[SerializeField] private Transform attackPoint;
	[SerializeField] private float attackPointXOffset = 0.6f;

	public int FacingSign { get; private set; } = 1; // +1 right, -1 left

	private void Reset()
	{
		targetFinder = GetComponent<BossTargetFinderView>();
		// Try to auto-find a child named Visual
		var visual = transform.Find("Visual");
		if (visual != null) visualRoot = visual;
		if (visualRoot != null) spriteRenderer = visualRoot.GetComponentInChildren<SpriteRenderer>(true);
		if (attackPoint == null)
		{
			var ap = transform.Find("AttackPoint");
			if (ap != null) attackPoint = ap;
		}
	}

	private void Update()
	{
		if (targetFinder == null) return;
		var t = targetFinder.CurrentTarget;
		if (t == null) return;

		float dx = t.position.x - transform.position.x;
		int desired = dx >= 0f ? 1 : -1;
		if (desired == FacingSign) return;

		SetFacing(desired);
	}

	public void SetFacing(int sign)
	{
		FacingSign = sign >= 0 ? 1 : -1;

		// Flip visuals only (don’t touch root physics)
		if (spriteRenderer != null)
		{
			spriteRenderer.flipX = (FacingSign < 0);
		}
		else if (visualRoot != null)
		{
			var s = visualRoot.localScale;
			s.x = Mathf.Abs(s.x) * FacingSign;
			visualRoot.localScale = s;
		}

		// Mirror attack point
		if (attackPoint != null)
		{
			var p = attackPoint.localPosition;
			p.x = Mathf.Abs(attackPointXOffset) * FacingSign;
			attackPoint.localPosition = p;
		}
	}
}