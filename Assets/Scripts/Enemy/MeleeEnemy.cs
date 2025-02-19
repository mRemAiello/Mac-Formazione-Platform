using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private TriggerMeleeAttack _triggerMeleeAttack;

    [Header("Audio")]
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private GameObject _audioSourcePrefab;

    public override void OnPostStart()
    {
        //
        _triggerMeleeAttack.Init(this, InternalEnemyData.MeleeDamage);
        _triggerMeleeAttack.gameObject.SetActive(false);
    }

    public override void MeleeAttack()
    {
        //
        if (!EnemyInSight)
            return;

        //
        if (IsAttacking)
            return;

        //
        if (IsRangedAttacking)
            return;

        // TODO: Controlla la y, 
        DistanceToEnemy = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (DistanceToEnemy <= InternalEnemyData.MeleeRange)
        {
            InternalRigidbody.linearVelocity = Vector2.zero;

            // TODO: Eventualmente scegli tra gli attacchi, con un intero che cambia l'attacco nell'Animator
            IsAttacking = true;

            //
            GameObject audioSource = Instantiate(_audioSourcePrefab, transform.position, Quaternion.identity);
            audioSource.GetComponent<AudioSource>().clip = _attackClip;
            audioSource.GetComponent<AudioSource>().Play();
        }
    }

    public override void OnPostDeath()
    {
    }

    public override void RangedAttack()
    {
    }

    public override void EndRangedAttack()
    {
    }

    public override void OnPostDrawGizmos()
    {
    }
}