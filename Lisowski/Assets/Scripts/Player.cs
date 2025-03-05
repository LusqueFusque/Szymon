using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1.6f;
    public float jumpForce = 4f;
    public float fallAnimationDelay = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool isAttacking;
    private bool isFalling;
    private bool isMoving;
    private bool startWalkCompleted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("Player initialized");
    }

    void Update()
    {
        if (isAttacking) return;
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateIdleState();
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        Debug.Log("Move input: " + move);

        if (move != 0 && isGrounded && !isFalling)
        {
            isMoving = true;
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
            spriteRenderer.flipX = move < 0;
            Debug.Log("Player moving");

            if (!startWalkCompleted && !anim.GetCurrentAnimatorStateInfo(0).IsName("StartWalk"))
            {
                anim.SetTrigger("StartWalk");
                Debug.Log("StartWalk animation triggered");
            }
            else
            {
                anim.SetBool("isWalking", true);
                Debug.Log("Walking animation active");
            }
        }
        else
        {
            isMoving = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            startWalkCompleted = false;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("isJumping", true);
            Debug.Log("Player jumped");
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            isFalling = true;
            anim.SetBool("isJumping", true);
            Debug.Log("Player falling");
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isAttacking = true;
            rb.velocity = Vector2.zero;
            anim.SetBool("isAttacking", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", false);
            anim.ResetTrigger("StartWalk");
            startWalkCompleted = false;
            Debug.Log("Player attacking");
            Invoke("EndAttack", 2.8f);
        }
    }

    void UpdateIdleState()
    {
        if (!isMoving && !isAttacking && isGrounded && !isFalling && !anim.GetBool("isJumping"))
        {
            anim.Play("Idle");
            Debug.Log("Idle animation active");
        }
    }

    public void OnStartWalkAnimationComplete()
    {
        startWalkCompleted = true;
        Debug.Log("StartWalk animation completed");
        if (isMoving)
        {
            anim.SetBool("isWalking", true);
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
        Debug.Log("Attack ended");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player grounded");
            if (isFalling)
            {
                Invoke("EndFallAnimation", fallAnimationDelay);
            }
            else
            {
                anim.SetBool("isJumping", false);
            }
        }
    }

    void EndFallAnimation()
    {
        isFalling = false;
        anim.SetBool("isJumping", false);
        Debug.Log("Fall animation ended");
    }
}
