using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Patrol")]
    [SerializeField] private GameObject _pointA;
    [SerializeField] private GameObject _pointB;

    //
    private Transform _currentPoint;
    private float _distanceToEnemy;
    private bool _enemyInSight = false;
    private bool _enemyInMeleeRange = false;

    void Start()
    {
        _currentPoint = _pointB.transform;
    }

    void Update()
    {
        //
        Patrol();

        //
        CheckEnemyInSight();

        // TODO: Attacco melee / ranged
        FollowEnemy();

        // TODO: 
        //MeleeAttack();

        // TODO: 
        //RangedAttack();

        // TODO: Aggro Range superato, torna indietro e fai Patrolling

        //
        UpdateAnimator();
    }

    private void Flip()
    {
        if (_spriteRenderer.flipX)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Patrol()
    {
        // 
        if (!_enemyData.CanPatrol)
            return;

        //
        if (_enemyInSight)
            return;

        //
        if (_currentPoint == _pointB.transform)
        {
            _rb.velocity = new Vector2(_enemyData.WalkSpeed, 0);
        }
        else
        {
            _rb.velocity = new Vector2(-_enemyData.WalkSpeed, 0);
        }

        //
        float distance = Vector2.Distance(transform.position, _currentPoint.position);
        if (distance < _enemyData.PatrolWaypointThreshold && _currentPoint == _pointB.transform)
        {
            Turn();
        }
        else if (distance < _enemyData.PatrolWaypointThreshold && _currentPoint == _pointA.transform)
        {
            Turn();
        }
    }

    private void Turn()
    {
        //
        _rb.velocity = Vector2.zero;

        //
        float timeToLaunch = Random.Range(_enemyData.TurnAnimationDurationMin, _enemyData.TurnAnimationDurationMax);

        //
        Invoke(nameof(EndTurn), timeToLaunch);
    }

    private void EndTurn()
    {
        float distance = Vector2.Distance(transform.position, _currentPoint.position);
        if (distance < _enemyData.PatrolWaypointThreshold && _currentPoint == _pointB.transform)
        {
            _currentPoint = _pointA.transform;
            Flip();
        }
        else if (distance < _enemyData.PatrolWaypointThreshold && _currentPoint == _pointA.transform)
        {
            _currentPoint = _pointB.transform;
            Flip();
        }
    }

    // TODO: Implementare knockback
    private void CheckEnemyInSight()
    {
        _distanceToEnemy = Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position);
        if (_distanceToEnemy < _enemyData.SightRadius)
        {
            _enemyInSight = true;
        }
        else
        {
            _enemyInSight = false;

            // TODO: impostare punto A o punto B
        }
    }

    void FollowEnemy()
    {
        // 
        if (!_enemyInSight)
            return;

        // TODO: Nemico in range di attacco, return

        //
        if (transform.position.x > PlayerMovement.Instance.transform.position.x)
        {
            _rb.velocity = new Vector2(-_enemyData.RunSpeed, 0); 
        }
        else
        {
            _rb.velocity = new Vector2(_enemyData.RunSpeed, 0); 
        }
    }

    private void UpdateAnimator()
    {
        //
        _animator.SetFloat("XVelocity", Mathf.Abs(_rb.velocity.x));

        //
        _animator.SetBool("EnemyInSight", _enemyInSight);
    }

    void OnDrawGizmos()
    {
        // Patrol
        if (_pointA != null)
            Gizmos.DrawWireSphere(_pointA.transform.position, 0.5f);
        if (_pointB != null)
            Gizmos.DrawWireSphere(_pointB.transform.position, 0.5f);
        if (_pointA != null && _pointB != null)
            Gizmos.DrawLine(_pointA.transform.position, _pointB.transform.position);

        // Sight
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _enemyData.SightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyData.MeleeRange);

        //
        Gizmos.color = Color.white;
    }
}