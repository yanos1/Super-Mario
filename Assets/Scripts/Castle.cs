using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public CastleFlag flag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Mario>() != null)
        {
            Mario mario = other.gameObject.GetComponent<Mario>();
            
            mario.gameObject.SetActive(false);
            StartCoroutine(GameManager.game.EndGameWin());
            StartCoroutine(flag.RaiseFlag());
            
        }
    }
}
