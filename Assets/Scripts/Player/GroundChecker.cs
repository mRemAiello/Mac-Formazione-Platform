using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    //
    public Rigidbody2D rb;

    // Layer che rappresenta il terreno
    public LayerMask groundLayer;

    // Oggetto che verifica il contatto con il terreno    
    public Transform groundCheck;

    // Raggio per controllare se il personaggio è sul terreno
    public float groundCheckRadius = 0.2f;

    // Soglia per considerare il personaggio in aria in base alla velocità verticale
    public float velocityThreshold = 0.1f; 

    //
    public bool isGrounded = false;

    protected void CheckIfIsGrounded()
    {
        // Controllo se il personaggio è a terra in base alla collisione e alla velocità verticale
        bool groundedByCollision = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Se è in contatto con il terreno e la velocità verticale è sufficientemente bassa, consideralo a terra
        if (groundedByCollision && Mathf.Abs(rb.velocity.y) <= velocityThreshold)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmos()
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