using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    // Layer che rappresenta il terreno
    public LayerMask groundLayer;

    // Oggetto che verifica il contatto con il terreno    
    public Transform groundCheck;

    // Raggio per controllare se il personaggio è sul terreno
    public float groundCheckRadius = 0.2f;

    //
    public bool isGrounded = false;

    void Update()
    {
        // Controllo se il personaggio è a terra
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            // Disegna il cerchio per visualizzare il controllo del terreno nell'Editor di Unity
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
            Gizmos.color = Color.white;
        }
    }
}