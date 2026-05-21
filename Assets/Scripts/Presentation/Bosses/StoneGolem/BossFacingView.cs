using UnityEngine;

/// <summary>
/// Flips the boss sprite to face its current target and mirrors the attack point
/// so the hitbox is always on the correct side.
///
/// Flipping strategy (in priority order):
///   1. If a SpriteRenderer is assigned, use spriteRenderer.flipX — this is the
///      safest method because it only affects the sprite, not the physics collider.
///   2. Otherwise flip the visualRoot's localScale.x — useful for multi-sprite rigs
///      where all children share one root transform.
///
/// IMPORTANT: Never flip the root GameObject's scale if it has a Rigidbody2D or
/// Collider2D — negative scale can invert physics calculations. Always flip a
/// dedicated "Visual" child instead.
/// </summary>
public class BossFacingView : MonoBehaviour
{
	[Header("Dependencies")]
	[SerializeField] private BossTargetFinderView targetFinder;

	[Header("Visual to flip")]
	/// <summary>The child transform that holds all sprites. Only this object is flipped.</summary>
	[SerializeField] private Transform visualRoot;
	/// <summary>Optional direct SpriteRenderer flip (preferred over scale flipping).</summary>
	[SerializeField] private SpriteRenderer spriteRenderer;

	[Header("Attack point mirroring")]
	/// <summary>The AttackPoint child whose X position mirrors the facing direction.</summary>
	[SerializeField] private Transform attackPoint;
	/// <summary>How far the attack point sits to the left/right of the boss pivot.</summary>
	[SerializeField] private float attackPointXOffset = 0.6f;

	/// <summary>Current facing direction. +1 = right, -1 = left.</summary>
	public int FacingSign { get; private set; } = 1;

	private void Reset()
	{
		targetFinder = GetComponent<BossTargetFinderView>();
		// Attempt to auto-find a child named "Visual"
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

		// Only update if direction actually changed to avoid redundant work
		if (desired == FacingSign) return;

		SetFacing(desired);
	}

	/// <summary>
	/// Forces the boss to face a given direction.
	/// Can be called externally (e.g. during a scripted cutscene).
	/// </summary>
	/// <param name="sign">+1 for right, -1 for left (any other value is clamped to -1).</param>
	public void SetFacing(int sign)
	{
		FacingSign = sign >= 0 ? 1 : -1;

		// Flip ONLY the visual layer — never the root — to avoid inverting colliders
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

		// Keep the attack point on the correct side so the damage circle
		// lands in front of the boss regardless of which way it faces
		if (attackPoint != null)
		{
			var p = attackPoint.localPosition;
			p.x = Mathf.Abs(attackPointXOffset) * FacingSign;
			attackPoint.localPosition = p;
		}
	}
}
