using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class HitFlash : MonoBehaviour
{
    [Header("Ref")]
    public HealthBehaviour health;
    private EnemyHealthView _enemyHealthView;

    //public SpriteRenderer spriteRenderer;
    public GameObject flashOverlay;

    [Header("Flash settings")]
    public Color flashColor =Color.white;
    public float flashDuration = 0.06f;
    public int flashes = 1;
    private Color _originalColor;
    private Coroutine _routine;

    void Awake()
    {
        if(health == null)
            health = GetComponent<HealthBehaviour>() ?? GetComponentInParent<HealthBehaviour>();
        _enemyHealthView = GetComponentInParent<EnemyHealthView>();

        if (flashOverlay != null)
            flashOverlay.SetActive(false);
    }

    void OnEnable()
    {
        // TEMP: support HealthView for now
        var hv = GetComponentInParent<HealthView>();
        if (hv != null)
            hv.Damaged += OnDamaged;

        if (_enemyHealthView != null)
            _enemyHealthView.Damaged += OnEnemyDamaged;

        if (health != null)
        {
            // Previously the Debug.Log below ran BEFORE the null check, which
            // threw a NullReferenceException on health.name when health was null.
            Debug.Log($"HitFlash subscribing to health on {health.name} (Id={health.GetInstanceID()})");
            health.Damaged += OnDamaged;
        }
        else
        {
            Debug.Log($"[HitFlash] No HealthBehaviour found on '{name}' — flash will only trigger via HealthView/EnemyHealthView.");
        }
    }

    void OnDisable()
    {
        if (health != null)
            health.Damaged -= OnDamaged;
        //TEMP: support HealthView for now
        var hv = GetComponentInParent<HealthView>();
        if (hv != null)
            hv.Damaged -= OnDamaged;

        if (_enemyHealthView != null)
            _enemyHealthView.Damaged -= OnEnemyDamaged;

    }

    void OnDamaged(int amount)
    {
        if (_routine != null)
            StopCoroutine(_routine);
        Debug.Log("FLASH!!!!!!!");
        _routine = StartCoroutine(FlashRoutine());
    }
    private void OnEnemyDamaged(int amount)
    {
        OnDamaged(amount);
    }

    public void ForceFlash()
    {
        if (_routine != null)
            StopCoroutine(_routine);
        _routine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        if (flashOverlay == null)
            yield break;
        for (int i =0; i < flashes; i++)
        {
            flashOverlay.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            flashOverlay.SetActive(false);
            yield return new WaitForSeconds(flashDuration);
        }

        _routine = null;
    }

}
