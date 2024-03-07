

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    // Start is called before the first frame update
    private Animator animator;
    private float direction = -1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
        
    {
        if(IsMarioCloseEnough() && !base.dead)
        {
            this.transform.position += (new Vector3(base.speed*direction, 0, 0))* Time.deltaTime;
        }
        DesroyIfOutOfBounds();
    }
    
    public override IEnumerator Die()
    {
        base.dead = true;
        animator.SetTrigger("die");
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Background");
        yield return new WaitForSeconds(2.5f);
        //Destroy(this.gameObject);     // why this gave bug?
        this.gameObject.SetActive(false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "GroundBlock")
        {
            direction *= -1;
        }
     }
    
}

