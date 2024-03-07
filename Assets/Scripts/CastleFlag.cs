using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleFlag : MonoBehaviour
{
    public IEnumerator RaiseFlag()
    {
        float duration = 1f;
        StartCoroutine(Util.DoLerp(gameObject, transform.position, transform.position + Vector3.up * 1.1f, duration));
        yield return new WaitForSeconds(duration);
    }

    // Start is called before the first frame update
 
}
