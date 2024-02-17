using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera:MonoBehaviour
{
    //static Coroutine coroutine;
    //public static void shakeCamera(float shakeTime, float shakeIntensity)
    //{
    //    if (coroutine != null)
    //        StopCoroutine(coroutine);
    //    coroutine = StartCoroutine(OnShakeCamera(shakeTime, shakeIntensity));
    //}

    //static IEnumerator OnShakeCamera(float shakeTime, float shakeIntensity)
    //{
    //    if (mainCam == null)
    //        mainCam = Camera.main;
    //    while (shakeTime > 0f)
    //    {
    //        mainCam.transform.position = mainCameraPos.position + Random.insideUnitSphere * shakeIntensity;
    //        shakeTime -= Time.deltaTime;

    //        yield return null;
    //    }

    //    mainCam.transform.position = mainCameraPos.position;
    //}
}
