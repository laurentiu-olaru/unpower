using UnityEngine;

public abstract class PickupView : MonoBehaviour
{
    protected IPickupEffect effect;

    protected abstract IPickupEffect CreateEffect();

    void Awake()
    {
        effect = CreateEffect();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var consumer = other.GetComponent<PlayerPickupConsumer>();
        if (consumer == null) return;

        effect.Apply(consumer.Context);
        Destroy(gameObject);
    }
}
