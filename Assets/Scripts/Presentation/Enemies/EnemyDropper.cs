using UnityEngine;

public class EnemyDropper : MonoBehaviour
{
    public GameObject pickupPrefab;
    [Range(0, 1)] public float dropChance = 1f;

    public void Drop()
    {
        if (pickupPrefab != null && Random.value <= dropChance)
            Instantiate(pickupPrefab, transform.position, Quaternion.identity);
    }
}
