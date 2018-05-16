using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGrabbable : MonoBehaviour
{
    public void Release()
    {
        StartCoroutine(Resize());
    }

    private IEnumerator Resize()
    {
        while (transform.localScale.x != 1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.025f);
            yield return new WaitForEndOfFrame();
        }
    }
}
