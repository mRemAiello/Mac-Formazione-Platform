using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Patrol")]
    [SerializeField] private bool _canPatrol = false;
    [SerializeField] private float _patrolWaypointThreshold = 0.5f;
    [SerializeField] private float _turnAnimationDurationMin = 0.3f;
    [SerializeField] private float _turnAnimationDurationMax = 0.3f;

    [Header("Melee Attack")]
    [SerializeField] private bool _canMeleeAttack = false;
    [SerializeField] private float _sightRadius = 4;
    [SerializeField] private float _meleeRange = 2;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 2;
    [SerializeField] private float _runSpeed = 4;

    //
    public bool CanPatrol => _canPatrol;
    public float TurnAnimationDurationMin => _turnAnimationDurationMin;
    public float TurnAnimationDurationMax => _turnAnimationDurationMax;
    public float PatrolWaypointThreshold => _patrolWaypointThreshold;
    public bool CanMeleeAttack => _canMeleeAttack;
    public float SightRadius => _sightRadius;
    public float MeleeRange => _meleeRange;
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
}