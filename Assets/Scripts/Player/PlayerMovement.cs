using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Velocità di movimento
    public float jumpForce = 10f;       // Forza del salto
    public float doubleJumpForce = 5f;
    public float gravityScale = 9.81f;   // Forza della gravità (personalizzabile)
    public float minXVelocity = 10;
    public float maxXVelocity = 10;
    public float minYVelocity = 10;
    public float maxYVelocity = 10;
    public int maxJumps = 2;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private GroundChecker groundChecker;
    private int jumpCount;
    private Vector2 velocity;
    private float moveInput;

    void Start()
    {
        // Ottiene il componente Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Ottiene il componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ottengo il component Ground Checker
        groundChecker = GetComponentInChildren<GroundChecker>();
    }

    void Update()
    {
        // Input orizzontale per il movimento (GetAxisRaw prende SOLO 0, 1, -1)
        // GetAxis prende anche valori intermedi (es. 0.01, -0.01)
        moveInput = Input.GetAxisRaw("Horizontal"); // Raccoglie l'input orizzontale (-1, 0, 1)
    
        // Muovi
        Move();

        // Controlla le condizioni di salto
        CheckJump();
    }

    private void LateUpdate()
    {
        // Gravità
        ApplyCustomGravity();

        // Limita velocità
        ClampVelocity2D();

        // Flip Sprite
        Flip();

        // Debug
        velocity = rb.velocity;

        // TODO: Inserire animazioni
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
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void CheckJump()
    {
        // Resetta il contatore dei salti se il personaggio è a terra
        if (groundChecker.isGrounded)
        {
            jumpCount = 0; // Resetta il numero di salti
        }

        // Salto e doppio salto
        if (Input.GetButtonDown("Jump") && (groundChecker.isGrounded || jumpCount < maxJumps - 1))
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Aggiunge forza verticale per il salto
        if (groundChecker.isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // TODO: Spawn del fumo, cambio animazione, suono
        }   

        // Doppio salto        
        if (!groundChecker.isGrounded && jumpCount < maxJumps - 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            // TODO: Spawn del fumo, cambio animazione, suono
        }            

        // Incrementa il numero di salti
        jumpCount++; 
    }

    // Metodo per applicare la gravità manualmente
    private void ApplyCustomGravity()
    {
        // Se il personaggio non è a terra, la gravità viene applicata
        if (!groundChecker.isGrounded) 
        {
            // Aggiunge la gravità alla velocità verticale
            rb.velocity += new Vector2(0, -gravityScale);
        }
    }

    private void ClampVelocity2D()
    {
        float clampedX = Mathf.Clamp(rb.velocity.x, minXVelocity, maxXVelocity);
        float clampedY = Mathf.Clamp(rb.velocity.y, minYVelocity, maxYVelocity);
        rb.velocity = new Vector2(clampedX, clampedY);
    }
}