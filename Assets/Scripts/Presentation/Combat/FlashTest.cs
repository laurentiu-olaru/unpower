using UnityEngine;
using System.Collections;
public class FlashTest : MonoBehaviour
{
    public GameObject flashOverlay;
    public float duration = 0.08f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        flashOverlay.SetActive(true);
        yield return new WaitForSeconds(duration);
        flashOverlay.SetActive(false);
    }
}
