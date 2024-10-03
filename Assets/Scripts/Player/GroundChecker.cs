using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    //
    [SerializeField] protected Rigidbody2D rb;

    // Layer che rappresenta il terreno
    [SerializeField] private LayerMask groundLayer;

    // Oggetto che verifica il contatto con il terreno    
    [SerializeField] private Transform _groundCheck;

    // Raggio per controllare se il personaggio è sul terreno
    [SerializeField] private float _groundCheckRadius = 0.2f;

    // Soglia per considerare il personaggio in aria in base alla velocità verticale
    [SerializeField] private float _velocityThreshold = 0.1f;

    //
    private bool _isGrounded = false;

    //
    public bool IsGrounded => _isGrounded;

    protected void CheckIfIsGrounded()
    {
        // Controllo se il personaggio è a terra in base alla collisione e alla velocità verticale
        bool groundedByCollision = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, groundLayer);

        // Se è in contatto con il terreno e la velocità verticale è sufficientemente bassa, consideralo a terra
        if (groundedByCollision && Mathf.Abs(rb.velocity.y) <= _velocityThreshold)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            // Disegna il cerchio per visualizzare il controllo del terreno nell'Editor di Unity
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_groundCheck.position, _groundCheckRadius);
            Gizmos.color = Color.white;
        }
    }
}