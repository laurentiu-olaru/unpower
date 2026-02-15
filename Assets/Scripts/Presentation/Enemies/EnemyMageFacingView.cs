using UnityEngine;

public class EnemyMageFacingView : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;

	// Optional: if your sprite faces left by default, invert this.
	[SerializeField] private bool spriteFacesRightByDefault = true;

	private bool hasAttackFacing;
	private float attackFacingUntilTime;

	void Awake()
	{
		if (spriteRenderer == null)
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	// Movement provides facing continuously.
	public void FaceMoveDirection(Vector2 moveDir)
	{
		if (Time.time < attackFacingUntilTime && hasAttackFacing)
			return; // attack facing has priority briefly

		ApplyFacing(moveDir);
	}

	// Attack provides facing at the moment of shooting.
	// We "lock" it for a short time so it doesn’t jitter if movement also updates that frame.
	public void FaceTargetDirection(Vector2 toTarget, float lockSeconds = 0.15f)
	{
		hasAttackFacing = true;
		attackFacingUntilTime = Time.time + Mathf.Max(0f, lockSeconds);
		ApplyFacing(toTarget);
	}

	private void ApplyFacing(Vector2 dir)
	{
		if (spriteRenderer == null) return;
		if (dir.x == 0f) return;

		bool shouldFaceRight = dir.x > 0f;
		if (!spriteFacesRightByDefault)
			shouldFaceRight = !shouldFaceRight;

		spriteRenderer.flipX = !shouldFaceRight;
		// Note: flipX logic depends on your sprite’s default facing.
		// If it flips the wrong way, toggle spriteFacesRightByDefault.
	}
}
