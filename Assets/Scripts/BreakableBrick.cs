using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBrick : MonoBehaviour, BaseBrick
{
    private Animator animator;
    private float hitCoolDown = 0f;


    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    public void Hit(GameObject obj)
    {
        if (hitCoolDown > 0) { return;}
        if (obj.CompareTag("SmallMario")) {
            animator.SetTrigger("hit");
            hitCoolDown = 0.3f;
            SoundManager.sounds.PlayBump();
        }

        else if (obj.CompareTag("BigMario"))
        {
            Destroy(this.gameObject);
            SoundManager.sounds.PlayBreakBlock();
          
        }
    }
    private void Update()
    {
        if (hitCoolDown > 0)
        {
            hitCoolDown -= Time.deltaTime;
        }
    }
}
