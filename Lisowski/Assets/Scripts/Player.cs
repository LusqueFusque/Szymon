using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool isAttacking;
    private bool isJumping;
    private bool isWalking;
    private bool isStartWalking;

    public float moveForce = 1.6f;
    public float jumpForce = 4f;
    public Transform groundCheck;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        if (isGrounded && isJumping)
        {

            isJumping = false;
            anim.SetBool("Jump", false);
            anim.SetBool("Idle", true);
        }

        if (isAttacking) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
            return;
        }

        if (isJumping || !isGrounded) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
            return;
        }

        Move();
    }

    private void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput == 0)
        {
            if (isStartWalking || isWalking)
            {
                isStartWalking = false;
                isWalking = false;
                anim.SetBool("StartWalk", false);
                anim.SetBool("Walk", false);
                anim.SetBool("Idle", true);
            }
            return;
        }

        if (!isWalking && !isStartWalking)
        {
            isStartWalking = true;
            anim.SetBool("Idle", false);
            anim.SetBool("StartWalk", true);
        }

        rb.velocity = new Vector2(moveInput * moveForce, rb.velocity.y);
        sr.flipX = moveInput < 0;
    }

    public void StartWalkTransition()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            isStartWalking = false;
            isWalking = true;
            anim.SetBool("StartWalk", false);
            anim.SetBool("Walk", true);
        }
        else
        {
            isStartWalking = false;
            anim.SetBool("StartWalk", false);
            anim.SetBool("Idle", true);
        }
    }

    private void Jump()
    {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetBool("Jump", true);
        anim.SetBool("Idle", false);
    }

    public void EndJump()
    {
        isJumping = false;
        anim.SetBool("Jump", false);
        anim.SetBool("Idle", true);
    }

    private void Attack()
    {
        isAttacking = true;
        anim.SetBool("Attack", true);
    }

    public void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", true);
    }

    public void OnStartWalkAnimationComplete()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            isStartWalking = false;
            isWalking = true;
            anim.SetBool("StartWalk", false);
            anim.SetBool("Walk", true);
        }
        else
        {
            isStartWalking = false;
            anim.SetBool("StartWalk", false);
            anim.SetBool("Idle", true);
        }
    }
    public void OnJumpAnimationComplete()
    {
        isJumping = false;
        anim.SetBool("Jump", false);
        anim.SetBool("Idle", true);
    }

    public void OnAttackAnimationComplete()
    {
        isAttacking = false;
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", true);
    }
    private void SetAnimationState(string state)
    {
        anim.SetBool("Idle", state == "Idle");
        anim.SetBool("StartWalk", state == "StartWalk");
        anim.SetBool("Walk", state == "Walk");
        anim.SetBool("Jump", state == "Jump");
        anim.SetBool("Attack", state == "Attack");
    }
}
