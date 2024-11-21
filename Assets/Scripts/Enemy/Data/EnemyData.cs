using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Sight")]
    [SerializeField] private float _sightRadius = 4;
    [SerializeField] private float _sightThresholdY = 3;
    [SerializeField] private float _aggroRange = 20;

    [Header("Patrol")]
    [SerializeField] private bool _canPatrol = false;
    [SerializeField] private float _patrolWaypointThreshold = 0.5f;
    [SerializeField] private float _turnAnimationDurationMin = 0.3f;
    [SerializeField] private float _turnAnimationDurationMax = 0.3f;

    [Header("Melee Attack")]
    [SerializeField] private bool _canMeleeAttack = false;
    [SerializeField] private float _meleeRange = 2;
    [SerializeField] private float _delayBetweenAttack = 0.5f;
    
    [Header("Ranged Attack")]
    [SerializeField] private bool _canRangedAttack = false;
    [SerializeField] private float _rangedSightRadius = 4;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 2;
    [SerializeField] private float _runSpeed = 4;

    [Header("Animations")]
    [SerializeField] private float _resetTime = 2.5f;

    //
    public bool CanPatrol => _canPatrol;
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
    public float MeleeRange => _meleeRange;
    public float DelayBetweenAttack => _delayBetweenAttack;
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float ResetTime => _resetTime;
}