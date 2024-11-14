using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    [Header("Patrol")]
    [SerializeField] private GameObject _pointA;
    [SerializeField] private GameObject _pointB;

    //
    private Transform _currentPoint;

    void Start()
    {
        _currentPoint = _pointA.transform;
    }

    void Update()
    {
        // 
        if (_enemyData.CanPatrol)
        {
            if (_currentPoint == _pointB.transform)
            {
                _rb.velocity = new Vector2(_enemyData.WalkSpeed, 0);
            }
            else
            {
                _rb.velocity = new Vector2(-_enemyData.WalkSpeed, 0);
            }

            //
            if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f && _currentPoint == _pointB.transform)
            {
                _currentPoint = _pointA.transform;
            }
            if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f && _currentPoint == _pointA.transform)
            {
                _currentPoint = _pointB.transform;
            }

            //
            _animator.SetFloat("XVelocity", _rb.velocity.x);
        }
    }

    void OnDrawGizmos()
    {
        if (_pointA != null)
            Gizmos.DrawWireSphere(_pointA.transform.position, 0.5f);
        if (_pointB != null)
            Gizmos.DrawWireSphere(_pointB.transform.position, 0.5f);
        if (_pointA != null && _pointB != null)
            Gizmos.DrawLine(_pointA.transform.position, _pointB.transform.position);
    }
}