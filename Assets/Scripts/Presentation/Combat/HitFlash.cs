using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class HitFlash : MonoBehaviour
{
    [Header("Ref")]
    public HealthBehaviour health;
    //public SpriteRenderer spriteRenderer;
    public GameObject flashOverlay;

    [Header("Flash settings")]
    public Color flashColor =Color.white;
    public float flashDuration = 0.06f;
    public int flashes = 2;
    private Color _originalColor;
    private Coroutine _routine;

    void Awake()
    {
        if(health == null)
            health = GetComponent<HealthBehaviour>() ?? GetComponentInParent<HealthBehaviour>();

        if (flashOverlay != null)
            flashOverlay.SetActive(false);
    }

    void OnEnable()
    {
        Debug.Log($"HitFlash OnEnableon {name}");
        if (health == null)
            Debug.Log($"Healts is null on {name}");

        Debug.Log($"HitFlash subscribing to health on {health.name} (Id={health.GetInstanceID()}");

        if (health != null)
            health.Damaged += OnDamaged; 


    }

    void OnDisable()
    {
        if (health != null)
            health.Damaged -= OnDamaged;
    }

    void OnDamaged(int amount)
    {
        if (_routine != null)
            StopCoroutine(_routine);
        Debug.Log("FLASH!!!!!!!");
        _routine = StartCoroutine(FlashRoutine());
    }

    public void ForceFlash()
    {
        if (_routine != null)
            StopCoroutine(_routine);
        _routine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
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
