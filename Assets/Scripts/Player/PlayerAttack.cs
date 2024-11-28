using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _attackCollider;
    private TriggerMeleeAttack _triggerMeleeAttack;
    [SerializeField] private PlayerMovementData _playerData;

    //
    private bool _isAttacking = false;

    void Start()
    {
        //_triggerMeleeAttack.Init(_playerData.Damage);
    }

    void Update()
    {
        if (_isAttacking)
            return;
        
        //
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Attiva animator mio, attiva animator slash
        _attackCollider.gameObject.SetActive(true);

        //
        _isAttacking = true;
    }
}