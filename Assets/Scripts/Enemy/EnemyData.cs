using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private bool _canPatrol = false;
    [SerializeField] private float _walkSpeed = 2;

    //
    public bool CanPatrol => _canPatrol;
    public float WalkSpeed => _walkSpeed;
}