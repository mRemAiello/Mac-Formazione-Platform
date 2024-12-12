using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMeleeAttack : MonoBehaviour
{
    [SerializeField] private string _enemyTag;

    //
    private IDamageable _sourceDamage;
    private float _damage;

    public void Init(IDamageable sourceDamage, float damage)
    {
        _sourceDamage = sourceDamage;
        _damage = damage;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_sourceDamage == null || _sourceDamage.IsDead)
            return;

        //
        if (!other.gameObject.tag.Equals(_enemyTag))
            return;

        //
        IDamageable damageable = other.GetComponent<IDamageable>();
        damageable ??= other.GetComponentInChildren<IDamageable>();

        //
        damageable?.TakeDamage(_damage);

        //
        IKnockable knockable = other.GetComponent<IKnockable>();
        knockable ??= other.GetComponentInChildren<IKnockable>();

        //
        if (knockable != null && knockable.IsKnockable)
        {
            knockable.KnockBack(transform, knockable.KnockBackForce, knockable.KnockBackTime, knockable.StunTime);
        }
    }
}