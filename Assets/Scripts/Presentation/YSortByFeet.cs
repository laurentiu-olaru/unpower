using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortByFeet : MonoBehaviour
{
    [Header("If set, we sort using this point (feet). Otherwise uses this object's transform.")]
    public Transform feetPoint;

    [Header("Higher = more precise sorting steps (try 100).")]
    public int sortingPrecision = 100;

    [Header("Use this to force something slightly in front/behind others at same Y.")]
    public int orderOffset = 0;

    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        float y = (feetPoint != null) ? feetPoint.position.y : transform.position.y;

        // Lower on screen (smaller y) should render in front => larger sortingOrder
        _sr.sortingOrder = -(Mathf.RoundToInt(y * sortingPrecision)) + orderOffset;
    }
}
