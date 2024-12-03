using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic")]
    [SerializeField] private float _hp;

    [Header("Knockback")]
    [SerializeField] private bool _isKnockable = true;
    [SerializeField] private float _knockBackForce = 2f;
    [SerializeField] private float _knockBackTime = 0.2f;
    [SerializeField] private float _stunTime = 0.4f;

    [Header("Sight")]
    [SerializeField] private float _sightRadius = 4;
    [SerializeField] private float _sightThresholdY = 3;
    [SerializeField] private float _aggroRange = 20;

    [Header("Patrol")]
    [SerializeField] private bool _canPatrol = false;
    [SerializeField] private bool _canFollow = true;
    [SerializeField] private float _patrolWaypointThreshold = 0.5f;
    [SerializeField] private float _turnAnimationDurationMin = 0.3f;
    [SerializeField] private float _turnAnimationDurationMax = 0.3f;

    [Header("Melee Attack")]
    [SerializeField] private bool _canMeleeAttack = false;
    [SerializeField] private float _meleeRange = 2;
    [SerializeField] private float _meleeDamage = 2;
    [SerializeField] private float _delayBetweenAttack = 0.5f;
    
    [Header("Ranged Attack")]
    [SerializeField] private bool _canRangedAttack = false;
    [SerializeField] private float _rangedAttackRange = 4;
    [SerializeField] private float _rangedDamage = 2;
    [SerializeField] private float _delayBetweenRangedAttack = 0.5f;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 2;
    [SerializeField] private float _runSpeed = 4;

    [Header("Animations")]
    [SerializeField] private float _resetTime = 2.5f;

    //
    public float HP => _hp;
    public bool IsKnockable => _isKnockable;
    public float KnockBackForce => _knockBackForce;
    public float KnockBackTime => _knockBackTime;
    public float StunTime => _stunTime;

    //
    public bool CanPatrol => _canPatrol;
    public bool CanFollow => _canFollow;
    public bool CanMeleeAttack => _canMeleeAttack;
    public bool CanRangedAttack => _canRangedAttack;

    //
    public float TurnAnimationDurationMin => _turnAnimationDurationMin;
    public float TurnAnimationDurationMax => _turnAnimationDurationMax;
    public float PatrolWaypointThreshold => _patrolWaypointThreshold;

    //
    public float AggroRange => _aggroRange;
    public float SightRadius => _sightRadius;
    public float SightThresholdY => _sightThresholdY;

    // Melee
    public float MeleeRange => _meleeRange;
    public float MeleeDamage => _meleeDamage;
    public float DelayBetweenAttack => _delayBetweenAttack;

    // Ranged
    public float RangedDamage => _rangedDamage;
    public float RangedAttackRange => _rangedAttackRange;
    public float DelayBetweenRangedAttack => _delayBetweenRangedAttack;

    // Movement
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float ResetTime => _resetTime;
}