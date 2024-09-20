using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Velocità di movimento
    public float jumpForce = 10f;       // Forza del salto
    public float doubleJumpForce = 5f;
    public float simpleGravity = 0.2f;
    public LayerMask groundLayer;       // Layer che rappresenta il terreno
    public Transform groundCheck;       // Oggetto che verifica il contatto con il terreno
    public float groundCheckRadius = 0.2f; // Raggio per controllare se il personaggio è sul terreno

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Ottiene il componente Rigidbody2D
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ottiene il componente SpriteRenderer
    }

    void Update()
    {
        // TODO: Inserire animazioni

        // Input orizzontale per il movimento (GetAxisRaw prende SOLO 0, 1, -1)
        // GetAxis prende anche valori intermedi (es. 0.01, -0.01)
        float moveInput = Input.GetAxisRaw("Horizontal"); // Raccoglie l'input orizzontale (-1, 0, 1)
        
        // Movimento del rigidbody impostando la velocità
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        //
        Debug.Log("Velocita 1: " + rb.velocity);

        // Flip della sprite in base alla direzione
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // Non capovolge la sprite
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true; // Capovolge la sprite
        }

        // Controllo se il personaggio è a terra
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aggiunge forza verticale per il salto
        }

        // Gravità
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - simpleGravity);

        Debug.Log("Velocita 2: " + rb.velocity);

        // Limito la velocità x e y
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            // Disegna il cerchio per visualizzare il controllo del terreno nell'Editor di Unity
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}