using System;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : GroundChecker
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 5f;
    public float gravityScale = 9.81f;
    public float minXVelocity = 10;
    public float maxXVelocity = 10;
    public float minYVelocity = 10;
    public float maxYVelocity = 10;
    public int maxJumps = 2;

    //
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private bool isJumping = false;
    private int jumpCount;
    private float moveInput;
    private bool isSlowed = false;
    private float slowJumpSpeed = 1;
    private float slowSpeed = 1;

    void Update()
    {
        // Input orizzontale per il movimento (GetAxisRaw prende SOLO 0, 1, -1)
        // GetAxis prende anche valori intermedi (es. 0.01, -0.01)
        moveInput = Input.GetAxisRaw("Horizontal"); // Raccoglie l'input orizzontale (-1, 0, 1)

        // Muovi
        Move();
    }

    private void LateUpdate()
    {
        // Controlla le condizioni di salto
        CheckJump();
        
        // Gravità
        ApplyCustomGravity();

        // Limita velocità
        ClampVelocity2D();

        // Flip Sprite
        Flip();

        //
        CheckIfIsGrounded();

        // Ricontrolla le condizioni di salto
        ResetJump();

        // Funzione che aggiorna l'animator
        UpdateAnimator();
    }

    private void Flip()
    {
        // Flip della sprite in base alla direzione
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // Non capovolge la sprite
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true; // Capovolge la sprite
        }
    }

    private void Move()
    {
        // Movimento del rigidbody impostando la velocità
        rb.velocity = new Vector2(moveInput * moveSpeed * slowSpeed, rb.velocity.y);
    }

    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * slowJumpSpeed);

                // Incrementa il numero di salti
                jumpCount++;

                // TODO: Spawn del fumo, cambio animazione, suono
            }
            // Doppio salto        
            else if (!isGrounded && jumpCount < maxJumps)
            {
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);

                // Incrementa il numero di salti
                jumpCount++; 

                // TODO: Spawn del fumo, cambio animazione, suono
            }
        }
    }

    private void ResetJump()
    {
        // Resetta il contatore dei salti se il personaggio è a terra
        if (isGrounded)
        {
            isJumping = false;
            // Resetta il numero di salti
            jumpCount = 0; 
        }
    }

    // Metodo per applicare la gravità manualmente
    private void ApplyCustomGravity()
    {
        // Se il personaggio non è a terra, la gravità viene applicata
        if (!isGrounded) 
        {
            // Aggiunge la gravità alla velocità verticale
            rb.velocity += new Vector2(0, -gravityScale);
        }
    }

    private void UpdateAnimator()
    {
        //
        animator.SetFloat("XVelocity", Math.Abs(rb.velocity.x));
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsSlowed", isSlowed);
    }

    private void ClampVelocity2D()
    {
        float clampedX = Mathf.Clamp(rb.velocity.x, minXVelocity, maxXVelocity);
        float clampedY = Mathf.Clamp(rb.velocity.y, minYVelocity, maxYVelocity);
        rb.velocity = new Vector2(clampedX, clampedY);
    }

    public void Slow(float slowSpeed, float slowJumpSpeed)
    {
        this.slowSpeed = slowSpeed;
        this.slowJumpSpeed = slowJumpSpeed;
        isSlowed = true;
    }

    public void RemoveSlow()
    {
        slowSpeed = 1;
        slowJumpSpeed = 1;
        isSlowed = false;
    }
}