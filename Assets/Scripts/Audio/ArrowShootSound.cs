using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ArrowShootSound : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip[] shootClips;
    [SerializeField, Range(0f, 1f)] private float volume = 0.6f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    private void Awake()
    {
        if (shootClips == null || shootClips.Length == 0)
            return;

        var source = GetComponent<AudioSource>();

        source.spatialBlend = 0f; // 2D sound
        source.playOnAwake = false;
        source.loop = false;

        source.clip = shootClips[Random.Range(0, shootClips.Length)];
        source.volume = volume;
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);

        source.Play();
    }
}
