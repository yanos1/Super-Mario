using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Koopa : Enemy
{
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    public Sprite deadSprite;
    private float direction = -1f;
    private float addedMovementSpeed = 0f;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    

    private void Update()   //fixed update?

    {
        if (IsMarioCloseEnough() && !dead)
        {
            transform.position += new Vector3((base.speed + addedMovementSpeed) * direction, 0, 0) * Time.deltaTime;
        }
        if (GetDistanceFromMario() > 20f  && dead)
        {
            Destroy(this.gameObject);
        }
        DesroyIfOutOfBounds();
    }
    public override IEnumerator Die()
    {
        base.dead = true;
        animator.enabled = false;
        spriteRenderer.sprite = deadSprite;

        //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        yield return null;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Enemy>() != null && base.dead && MathF.Abs(rb.velocity.x) > 0.1f)     // is this efficient ?
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.Die());
            int movedir;
            if (enemy.transform.position.x < gameObject.transform.position.x)
            {
                movedir = -1;
            }
            else
            {
                movedir = 1;
            }
            AddVelocity(movedir);
        }
        if (col.gameObject.tag != "GroundBlock" && !base.dead)
        {
            direction *= -1;
        }
        /*
        else if (other.gameObject.GetComponent<Mario>() != null && base.dead)
        {
            Mario mario = other.gameObject.GetComponent<Mario>();
            if (Mathf.Abs(rb.velocity.x) <= 0.01)
            {
                int movedir;
                if (mario.transform.position.x < gameObject.transform.position.x)
                {
                    movedir = 1;
                }
                else
                {
                    movedir = -1;
                }
                AddVelocity(movedir);            }
            else
            {
                mario.Die();
            }
        }
        */


        //else if (other.gameObject.GetComponent<Mario>() != null && !base.dead)
        //{
        //    Mario mario = other.gameObject.GetComponent<Mario>();
        //    mario.Die();
        //} 
    }

    private void AddVelocity(int movedir)
    {

        //rb.velocity = new Vector3(20f*movedir,0,0);
        rb.AddForce(50f * movedir, 0, 0);
    }
    //public bool CanInteractWithMario()
    //{
    //    if(GetDistanceFromMario() < pickUpDistance && base.dead)
    //    {
    //        return true;
    //    }
    //    return false;
    //}


  


    // Start is called before the first frame update

}
