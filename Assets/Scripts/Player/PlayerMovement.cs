using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocità di movimento
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Ottiene il componente Rigidbody2D
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ottiene il componente SpriteRenderer
    }

    void Update()
    {
        // TODO: Inserire animazioni
        // Input orizzontale per il movimento (GetAxisRaw prende SOLO 0, 1, -1)ù
        // GetAxis prende anche valori intermedi (es. 0.01, -0.01)
        float moveInput = Input.GetAxisRaw("Horizontal"); // Raccoglie l'input orizzontale (-1, 0, 1)
        
        // Movimento del rigidbody impostando la velocità
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip della sprite in base alla direzione
        if (moveInput > 0)
        {
            spriteRenderer.flipX = true; // Non capovolge la sprite
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = false; // Capovolge la sprite
        }

        // Hai premuto spacebar?
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit");
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        Debug.Log("Stay");
    }
}