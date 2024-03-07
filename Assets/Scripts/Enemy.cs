using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private const float START_MOVING_DISTANCE = 24f;
    private const float OUT_OF_SCREEN_DISTANCE = -15f;
    public Mario mario;
    public float speed = 2f;
    public bool dead = false;



    public abstract IEnumerator Die();

    public bool IsMarioCloseEnough()
    {
        float ditance = GetDistanceFromMario();
        return ditance < START_MOVING_DISTANCE;
    }

    public void DesroyIfOutOfBounds()
    {
        if (this.gameObject.transform.position.y < GameManager.OUT_OF_BOUNDS_POSITION)
        {   
           gameObject.gameObject.SetActive(false);
        }
        else if (GetDistanceFromMario() < OUT_OF_SCREEN_DISTANCE)
        {
            gameObject.gameObject.SetActive(false);

        }
    }
    public float GetDistanceFromMario()
{
        
    for (int i = 0; i < mario.transform.childCount; i++)
    {
        Transform childTransform = mario.transform.GetChild(i);
        if (childTransform.gameObject.activeSelf)
          {
                return this.transform.position.x - childTransform.gameObject.transform.position.x;
                
        }
    }
    return 0;

}



}