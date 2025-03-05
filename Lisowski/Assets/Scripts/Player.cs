using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.6f;
    public float jumpForce = 4f;
    private bool isGrounded;
    private bool isAttacking;
    private bool isFalling;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public float fallAnimationDelay = 0.2f; // Tempo para finalizar a animação de queda

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isAttacking) return; // Bloqueia movimentação durante o ataque

        float move = Input.GetAxisRaw("Horizontal");

        if (move != 0 && isGrounded && !isFalling)
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
            spriteRenderer.flipX = move < 0; // Gira o sprite corretamente
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("isJumping", true);
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            isFalling = true;
            anim.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Para a movimentação durante o ataque
        anim.SetBool("isAttacking", true);
        anim.SetBool("isWalking", false);
        anim.SetBool("isJumping", false);
        Invoke("EndAttack", 2.8f); // Tempo da animação de ataque
    }

    void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (isFalling)
            {
                Invoke("EndFallAnimation", fallAnimationDelay); // Dá tempo para a animação de queda terminar
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
    }
}
