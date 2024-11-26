using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    //
    private bool _isRangeAttacking = false;

    public override void MeleeAttack()
    {
    }

    public override void OnPostDeath()
    {
    }

    public override void OnPostStart()
    {
        throw new System.NotImplementedException();
    }

    public override void RangedAttack()
    {
        _isRangeAttacking = true;
    }

    public void SpawnProjectile()
    {

    }
}