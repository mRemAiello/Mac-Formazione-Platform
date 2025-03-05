using UnityEngine;

public class Egg : MonoBehaviour, IDamageable
{
    public float frictionAmount = 0.1f;
    public Rigidbody2D _rigidbody;

    public Transform _groundCheck;
    public float _groundCheckRadius;
    public LayerMask _groundLayer;

    // Altezza minima di caduta per infliggere danni
    public float minFallHeight = 5.0f;

    // Danno per ogni metro oltre l'altezza minima
    public float damagePerMeter = 10.0f;

    // Altezza di partenza della caduta
    private float fallStartY;

    // Se il giocatore sta cadendo
    private bool isFalling = false;

    // Salute attuale del giocatore
    public int CurrentHealth;

    public bool IsDead => CurrentHealth <= 0;
    public bool IsAlive => CurrentHealth > 0;

    private void Update()
    {
        if (!IsGrounded()) // Se non è a terra, è in caduta
        {
            if (!isFalling) // Inizia a tracciare la caduta
            {
                isFalling = true;
                fallStartY = transform.position.y;
            }
        }
        else // Il giocatore è atterrato
        {
            if (isFalling) // Se era in caduta prima di atterrare
            {
                float fallDistance = fallStartY - transform.position.y;

                if (fallDistance > minFallHeight) // Controlla se la caduta supera l'altezza minima
                {
                    float damage = (fallDistance - minFallHeight) * damagePerMeter;
                    TakeDamage(damage);
                }

                isFalling = false; // Resetta lo stato di caduta
            }
        }
    }

    private void FixedUpdate()
    {
        // Applica la frizione alla velocità angolare
        ApplyRollingFriction();
    }

    void ApplyRollingFriction()
    {
        // Se l'oggetto ha una velocità angolare, applica la frizione
        if (_rigidbody.angularVelocity > 0)
        {
            // Riduci la velocità angolare in base alla quantità di frizione e al tempo trascorso
            float newAngularVelocity = _rigidbody.angularVelocity * (1 - frictionAmount * Time.fixedDeltaTime);

            // Ferma l'oggetto completamente se la velocità è molto bassa (soglia di arresto)
            if (newAngularVelocity < 0.01f)
            {
                newAngularVelocity = 0; // Arresta completamente la rotazione
            }

            // Aggiorna la velocità angolare dell'oggetto
            _rigidbody.angularVelocity = newAngularVelocity;
        }
    }

    public bool IsGrounded()
    {
        // Controllo se il personaggio è a terra in base alla collisione e alla velocità verticale
        return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= (int)damage;
        if (IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Sostituisci con l'animazione e la sprite dell'uovo distrutto
    }
}