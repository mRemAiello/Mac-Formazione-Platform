using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMeleeAttack : MonoBehaviour
{
    [SerializeField] private string _enemyTag;

    //
    private float _damage;

    public void Init(float damage)
    {
        _damage = damage;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals(_enemyTag))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable ??= other.GetComponentInChildren<IDamageable>();

            //
            damageable?.TakeDamage(_damage);

            // TODO: Knockback
        }
    }
}