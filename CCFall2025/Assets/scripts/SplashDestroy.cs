using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;

public class SplashDestroy : MonoBehaviour
{
    public void DestroySplash()
    {
        StartCoroutine(DestroyAfterDelay());
    }
    IEnumerator DestroyAfterDelay()
    {
        GetComponent<VisualEffect>().SendEvent("OnPlay");
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    } 
}
