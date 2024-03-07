using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBrick : MonoBehaviour 
{
    public EmptyBrick emptyBrick;
    
    public void Deactivate()
    {
        Instantiate(emptyBrick, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        
    }
}
