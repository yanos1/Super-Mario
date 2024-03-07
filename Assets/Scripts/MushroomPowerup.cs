using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MushroomPowerup : MonoBehaviour
{

    private float speed = 3f;
    private int direction = 1;
    private float spawningDuration = 1f;
    bool materilize = false;
    Rigidbody rb;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (spawningDuration >=0) {
            spawningDuration -= Time.deltaTime;
            return;
        }
        if ( !materilize)
        {   
            materilize=true;
            rb.isKinematic = false;
         
        }
        this.transform.position += (new Vector3(speed*direction, 0, 0)) * Time.deltaTime;
        if (this.gameObject.transform.position.y < GameManager.OUT_OF_BOUNDS_POSITION)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pipe") || collision.collider.CompareTag("Enemy"))
        {
            direction *= -1;
        }
       
    }

}
