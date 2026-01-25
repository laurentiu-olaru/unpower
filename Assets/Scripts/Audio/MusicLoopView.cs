using UnityEngine;

public class MusicLoopView : MonoBehaviour
{
    private static MusicLoopView instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
