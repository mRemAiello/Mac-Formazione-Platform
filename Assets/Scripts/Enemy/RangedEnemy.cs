using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Projectile")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _spawnPosition;

    //
    public override void MeleeAttack()
    {
    }

    public override void OnPostDeath()
    {
    }

    public override void OnPostStart()
    {
    }

    public override void RangedAttack()
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
        if (DistanceToEnemy <= InternalEnemyData.RangedAttackRange)
        {
            InternalRigidbody.linearVelocity = Vector2.zero;

            // TODO: Eventualmente scegli tra gli attacchi, con un intero che cambia l'attacco nell'Animator
            IsRangedAttacking = true;
        }
    }

    public override void EndRangedAttack()
    {
        IsRangedAttacking = false;
    }

    public void SpawnProjectile()
    {
        GameObject obj = Instantiate(_projectile, _spawnPosition.position, Quaternion.identity);
        FireProjectile proj = obj.GetComponent<FireProjectile>();

        // TODO: Interfaccia
        /*Vector3 position = Vector3.zero;
        position.x = PlayerController.Instance.gameObject.transform.position.x;
        position.y = transform.position.y;
        proj.Seek(position);*/
    }

    public override void OnPostDrawGizmos()
    {
        // Sight
        if (_enemyData != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _enemyData.RangedAttackRange);
            Gizmos.color = Color.white;
        }
    }
}