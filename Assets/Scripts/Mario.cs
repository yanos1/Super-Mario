using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Mario : MonoBehaviour
{
    private const float MAX_MOVEMENT_SPEED = 6f;
    public Mario mario;

    public Camera cam;
    public CharacterController controller;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Sprite deadSprite;
    private GameObject canInteractWith;
    private GameObject interactingWith;
    private float jumpForce;
    private float jumpForceAdditionInAir;
    private float jumpStartTime;
    private float extraJumpTimeLeft;
    public float baseMovementSpeed;
    private bool isJumping;
    private bool grounded;
    public float leftspeedAddition;
    public float rightspeedAddition;
    private bool isDead = false;
    private bool finished = false;
    private bool canPickUp = false;

    public float pickUpRange = 1f;
    public float dropForwardSpeed = 100f;
    public bool equipped = false;
    public static bool slotFull = false;

    //how to arrange all of this ??
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        grounded = false;
        isJumping = false;
        jumpForce = 6.7f;
        extraJumpTimeLeft = 0;
        jumpStartTime = 2f;
        jumpForceAdditionInAir = 1.8f;
        baseMovementSpeed = 2f;
        leftspeedAddition = 0f;
        rightspeedAddition = 0f;
    }
    void Update()
    {
        if (!this.controller.enabled) { return; }
        HandleSpeedAddition();
        Vector3 velocity = HandleMovement();
        HandleAnimation();
        HandleDirection();
        HandleFalling();
        bool hit = HandleCollision();

        if (hit) {velocity = new Vector3(velocity.x, 0, 0);}
        if (GameManager.game.getTimeLeft() <= 0) { Die(); }
        if (CantMove(velocity)) {return;}
        if (equipped)
        {
            interactingWith.transform.position = transform.position + (Vector3.up * controller.height / 2f)+Vector3.up;
        }
        else if(canInteractWith != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && !slotFull) { PickUp(); }
        }
        if (equipped && Input.GetKeyDown(KeyCode.Q)) { Drop(); }


        controller.Move((velocity + GameManager.game.AddGravity()) * Time.deltaTime);    // talk with alon about this not in fiexdupdate
    }

    public void Die()
    {
        if (GameManager.game.GetLevel() == 0)
        {
            GameManager.game.LoseHealth();
            StartCoroutine(DieAnimation());
            return;
        }
        GameManager.game.SetLevel(0);
        SoundManager.sounds.PlayLosePowerup();
        SwapCharacters(false, true);
    }

    public IEnumerator HitFlagPole(Vector3 flagBottomPosition)
    {
        this.gameObject.GetComponent<CharacterController>().enabled = false;

        float duration = 0.8f;
        StartCoroutine(Util.DoLerp(gameObject, transform.position, flagBottomPosition, duration));
        yield return new WaitForSeconds(duration);
        StartCoroutine(Util.DoLerp(gameObject, transform.position, transform.position + (Vector3.right)*1f, duration/5));
        yield return new WaitForSeconds(duration/5);
        finished = true;
        SoundManager.sounds.PlayStageClear();

        this.gameObject.GetComponent<CharacterController>().enabled = true;
    }

    private bool CantMove(Vector3 velocity)
    {
        return !IsWhithinCameraLeftBound() && velocity.x < 0 ||
            !this.controller.enabled ||
            !this.gameObject.activeSelf;
;
    }

    private void HandleFalling()
    {   
        if (isDead) { return; }
        if (transform.position.y < GameManager.OUT_OF_BOUNDS_POSITION)
        {
            isDead = true;
            GameManager.game.SetLevel(0);
            GameManager.game.LoseHealth();
        }
    }

    private bool IsWhithinCameraLeftBound()
    {
        Vector3 leftViewportPoint = new Vector3(0, 0.5f, cam.nearClipPlane);
        Vector3 leftWorldPoint = cam.ScreenToWorldPoint(leftViewportPoint);

        if (leftWorldPoint.x + 0.1f >= this.transform.position.x)
        {
            return false;
        }
        return true;
    }
    private bool HandleCollision()
    {
        float height = controller.height;
        RaycastHit hit;

        if (Physics.BoxCast(transform.position, Vector3.one * 0.5f, Vector3.up, out hit,
            Quaternion.identity, height / 2, LayerMask.GetMask("Default")))  
        {
            if (hit.collider.gameObject.GetComponent<BaseBrick>() != null)
            {
                BaseBrick brick = hit.collider.gameObject.GetComponent<BaseBrick>();
                if (brick != null)
                {
                    // Call a method that is defined in the BaseBrick interface or base class
                    brick.Hit(this.gameObject);
                    return true;
                }
            }
            DetectEnemyHit(hit);
        }
        if (Physics.BoxCast(transform.position, Vector3.one * 0.5f, Vector3.right,
            out hit, Quaternion.identity, 0.15f, LayerMask.GetMask("Default")) ||
            Physics.BoxCast(transform.position, Vector3.one * 0.5f, Vector3.left,
            out hit, Quaternion.identity, 0.15f, LayerMask.GetMask("Default")))
        {
            DetectEnemyHit(hit);
        }
        else if (Physics.BoxCast(transform.position, Vector3.one * 0.5f,
            Vector3.down, out hit, Quaternion.identity, 0.7f, LayerMask.GetMask("Default"))) {
            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                bool killed = KillEnemy(hit);
                return killed;
            }
        }
        if (Physics.BoxCast(transform.position, Vector3.one * 0.5f, Vector3.right,
           out hit, Quaternion.identity, 0.5f, LayerMask.GetMask("Default")))
        {
            if (hit.collider.gameObject.CompareTag("Koopa"))  
            {
                canInteractWith = hit.collider.gameObject;
                interactingWith = canInteractWith;
            }
            else
            {
                canPickUp = false;
                canInteractWith = null;
            }
            
        }
        return false;
    }
    private bool KillEnemy(RaycastHit hit)
    {
        Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
        if (enemy != null && !enemy.dead)
        {
            GameManager.game.AddScore(100);
            SoundManager.sounds.PlayKill();
            StartCoroutine(enemy.Die());
            float duration = 0.2f;
            StartCoroutine(Util.DoLerp(gameObject, transform.position, transform.position + new Vector3(0, 0.5f, 0), duration));
            return true;
        }
        return false;
    }
    private void DetectEnemyHit(RaycastHit hit)
    {
        if (hit.collider.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (!enemy.dead)
            {
                Die();
                Destroy(hit.collider.gameObject);
            }
        }
    }

    IEnumerator DieAnimation()
    {
        this.gameObject.GetComponent<CharacterController>().enabled = false;
        animator.enabled = false;
        spriteRenderer.sprite = deadSprite;

        float duration = 0.4f;
        StartCoroutine(Util.DoLerp(gameObject, transform.position, transform.position + Vector3.up * 3, duration));
        yield return new WaitForSeconds(duration);
        StartCoroutine(Util.DoLerp(gameObject, transform.position, transform.position + Vector3.down * 12, duration*2));
        yield return new WaitForSeconds(duration);
    }

    
    private void HandleDirection()
    {
        if ( Input.GetAxisRaw("Horizontal") > 0)
            
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if ( Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.eulerAngles = finished ? Vector3.zero :new Vector3(0f, 180f, 0f);
        }
    }

    private void HandleAnimation()
    {
        if (grounded != controller.isGrounded)
        {
            animator.SetBool("grounded", controller.isGrounded);  // ask if this reduced points
            grounded = controller.isGrounded;
        }
        animator.SetFloat("velX", finished ? 1 :Mathf.Abs(Input.GetAxis("Horizontal")));

        if (controller.velocity.x == 0 && Mathf.Abs(Input.GetAxis("Horizontal")) ==0)
        {
            animator.SetTrigger("notMoving");
        }
        if ((controller.velocity.x >= 0 && Input.GetAxis("Horizontal") >= 0) || (controller.velocity.x <=0 && Input.GetAxis("Horizontal") <= 0))   //// the slide is not on poing, the soop movement too. 
        {
            animator.SetBool("slide", false);
        }
        else if (rightspeedAddition > 0 && Input.GetKey(KeyCode.LeftArrow)) {
            animator.SetBool("slide", true);

        }
        else if (leftspeedAddition < 0 && Input.GetKey(KeyCode.RightArrow)) {
            animator.SetBool("slide", true);
        }

    }

    private Vector3 HandleMovement()
    {
        Vector3 velocity = new Vector3(baseMovementSpeed*Input.GetAxis("Horizontal") + leftspeedAddition+rightspeedAddition, controller.velocity.y, 0); 
        if (finished)
        {
            velocity = new Vector3(4.5f, controller.velocity.y, 0);

            return velocity;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && grounded)
        {
            isJumping = true;
            extraJumpTimeLeft = jumpStartTime;
            velocity += Vector3.up * jumpForce;
            SoundManager.sounds.PlayJump();
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && isJumping)
        {
            if (extraJumpTimeLeft > 0f)
            {
                velocity += Vector3.up * jumpForceAdditionInAir;
                extraJumpTimeLeft -= Time.deltaTime * 10;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }
        return velocity;
    }

    private void HandleSpeedAddition()
    {
        
        if (Input.GetAxis("Horizontal") > 0)

        {
            if (rightspeedAddition < MAX_MOVEMENT_SPEED)
            {
                rightspeedAddition +=  3*Time.deltaTime;
            }
            leftspeedAddition = Mathf.Min(leftspeedAddition +MAX_MOVEMENT_SPEED * Time.deltaTime, 0);

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            {
                if (leftspeedAddition >-MAX_MOVEMENT_SPEED )
                {
                    leftspeedAddition -= 3 * Time.deltaTime;
                }
                rightspeedAddition = Mathf.Max(rightspeedAddition- MAX_MOVEMENT_SPEED * Time.deltaTime, 0);
            }
        }
        else
        {
            rightspeedAddition = Mathf.Max(rightspeedAddition - (2f*MAX_MOVEMENT_SPEED) * Time.deltaTime, 0);
            leftspeedAddition = Mathf.Min(leftspeedAddition + (2f * MAX_MOVEMENT_SPEED) * Time.deltaTime, 0);
        }

    }

    private void SwapCharacters(bool swapping, bool swapped)
    {
        if (equipped) { Destroy(interactingWith); equipped = false; slotFull = false; }
        Transform parentTransform = mario.transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            // Get the child Transform at index i
            Transform childTransform = parentTransform.GetChild(i);
            if (childTransform.gameObject.CompareTag("SmallMario") == true)
            {
                childTransform.gameObject.SetActive(swapped);
                childTransform.gameObject.GetComponent<CharacterController>().enabled = false;
                childTransform.gameObject.transform.position = transform.position;
                childTransform.gameObject.GetComponent<CharacterController>().enabled = true;
            }
            if (childTransform.gameObject.CompareTag("BigMario") == true)
            {
                childTransform.gameObject.SetActive(swapping);
                childTransform.gameObject.GetComponent<CharacterController>().enabled = false;
                childTransform.gameObject.transform.position = transform.position;
                childTransform.gameObject.GetComponent<CharacterController>().enabled = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mushroom"))
        {
            SwapCharacters(true, false);
            GameManager.game.AddLevel();
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Flower"))
        {
            // power up
            GameManager.game.AddLevel();
            Destroy(other.gameObject);
        }
    }
    private void Drop()
    {
        equipped = false;
        slotFull = false;
        Rigidbody rb = interactingWith.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Vector3.right * dropForwardSpeed);
        interactingWith = null;
        jumpForce -= 3;
    }

    private void PickUp()
    {
        interactingWith = canInteractWith;
        Rigidbody rb = interactingWith.GetComponent<Rigidbody>();
        equipped = true;
        slotFull = true;
        rb.isKinematic = true;
        interactingWith.transform.position = transform.position + (Vector3.up * controller.height / 2f) + Vector3.up;
        jumpForce+=3;


    }








}
