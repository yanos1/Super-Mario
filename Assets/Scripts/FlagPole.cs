using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Flag flag;
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.GetComponent<Mario>() != null)
        {
            Mario mario = other.gameObject.GetComponent<Mario>();
            StartCoroutine(mario.HitFlagPole(this.transform.position- new Vector3(0,3,0)));
            StartCoroutine(flag.GetFLagDown(new Vector3(182.78f,-4.7f,0f)));
        }
    }
}
