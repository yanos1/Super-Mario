using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public  IEnumerator GetFLagDown(Vector3 flagBottomPosition)
    {
        float duration = 0.8f;
        SoundManager.sounds.PlayFlapPole();
        StartCoroutine(Util.DoLerp(gameObject, transform.position, flagBottomPosition, duration));
        yield return new WaitForSeconds(duration);
    }

    
}
