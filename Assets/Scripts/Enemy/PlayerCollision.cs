using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float knockBackForce = 15f; // Forza di rimbalzo
    public float knockBackDuration = 0.5f; // Durata dell'effetto di rimbalzo

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Controlla se il personaggio collide con un nemico
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Calcola la direzione di rimbalzo
            Vector2 knockBackDirection = (transform.position - collision.transform.position).normalized;

            // Applica la forza di rimbalzo
            rb.linearVelocity = knockBackDirection * knockBackForce;

            // Puoi anche usare un Coroutine per gestire il knockback
            StartCoroutine(ResetVelocityAfterTime());
        }
    }

    private IEnumerator ResetVelocityAfterTime()
    {
        // Aspetta per la durata dell'effetto di rimbalzo
        yield return new WaitForSeconds(knockBackDuration);

        // Resetta la velocità del rigidbody
        rb.linearVelocity = Vector2.zero;
    }
}