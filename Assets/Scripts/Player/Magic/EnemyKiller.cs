using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiller : MonoBehaviour
{
    public float attackRange = 20f; // Distanza per attaccare
    public LayerMask enemyLayer; // Layer dei nemici
    public Animator animator; // Riferimento all'Animator

    private bool isAttacking = false; // Flag per controllare se l'attacco è in corso

    void Update()
    {
        // Controlla se l'animazione attuale è "Idle" prima di consentire l'attacco
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // Se premi il tasto R e non sei già in attacco
            if (Input.GetKeyDown(KeyCode.R) && !isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true; // Imposta il flag a true per segnare l'inizio dell'attacco
        KillEnemiesInRange();

        // Riproduci l'animazione di attacco
        if (animator != null)
        {
            animator.SetTrigger("MagicAttackR"); // Assicurati di avere un trigger "MagicAttackR" nell'Animator
        }

        // Aspetta fino a quando l'animazione di attacco è finita
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")); // Controlla che l'animazione "Idle" sia in corso

        isAttacking = false; // Ripristina il flag a false
    }

    void KillEnemiesInRange()
    {
        // Controlla i nemici all'interno dell'area di attacco
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        
        foreach (var enemy in enemies)
        {
            // Assume che i nemici abbiano uno script chiamato Enemy che gestisce la loro morte
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                //enemyScript.Die(); // Chiama il metodo di morte del nemico
            }
        }
    }

    // Disegna il cerchio dell'area di attacco nel Scene View (per debug)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}