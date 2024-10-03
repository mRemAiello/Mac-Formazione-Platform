using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player Movement")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Speed")]
    [SerializeField, Range(1, 99)] private float _moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _doubleJumpForce = 5f;
    [SerializeField] private float _jumpDelay = 0.1f;
    [SerializeField] private int _maxJumps = 2;

    [Header("Gravity")]
    [SerializeField] private float _gravityScale = 2f;
    
    [Header("Velocity")]
    [SerializeField] private float _minXVelocity = 10;
    [SerializeField] private float _maxXVelocity = 10;
    [SerializeField] private float _minYVelocity = 10;
    [SerializeField] private float _maxYVelocity = 10;

    // Property
    public float MoveSpeed => _moveSpeed;
    public float JumpForce => _jumpForce;
    public float DoubleJumpForce => _doubleJumpForce;
    public float JumpDelay => _jumpDelay;
    public float GravityScale => _gravityScale;
    public float MinXVelocity => _minXVelocity;
    public float MaxXVelocity => _maxXVelocity;
    public float MinYVelocity => _minYVelocity;
    public float MaxYVelocity => _maxYVelocity;
    public int MaxJumps => _maxJumps;
}