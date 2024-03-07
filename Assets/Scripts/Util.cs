using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static IEnumerator DoLerp(GameObject obj, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float precentageCompleted = elapsedTime / duration;
            obj.transform.position = Vector3.Lerp(startPos, endPos, precentageCompleted);
            yield return null;
        }
    }
}
