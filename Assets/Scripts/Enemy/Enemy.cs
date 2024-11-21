using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Patrol")]
    [SerializeField] private GameObject _startingPoint;
    [SerializeField] private GameObject _pointA;
    [SerializeField] private GameObject _pointB;

    [Header("Melee Attack")]
    [SerializeField] private GameObject _attackColliderGameObject;

    [Header("VFX")]
    [SerializeField] private GameObject _smokeVFX;

    //
    private EnemyOrientation _enemyOrientation = EnemyOrientation.Right;
    private Transform _currentPoint;
    private float _distanceToEnemy;
    private bool _wasPrevInSight = false;
    private bool _enemyInSight = false;
    private bool _enemyInMeleeRange = false;
    private bool _isDeath = false;
    private bool _isAttacking = false;
    private bool _forceDisable = false;

    //
    void Start()
    {
        _currentPoint = _pointB.transform;
    }

    void Update()
    {
        //
        if (_isDeath)
            return;

        //
        if (_forceDisable)
            return;

        //
        Patrol();

        //
        CheckEnemyInSight();

        // TODO: Attacco melee / ranged
        FollowEnemy();

        //  
        MeleeAttack();

        // TODO: 
        //RangedAttack();

        //
        CheckAggroDistance();
    }

    void LateUpdate()
    {
        //
        UpdateAnimator();
    }

    private void Flip()
    {
        if (_enemyOrientation == EnemyOrientation.Left)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _enemyOrientation = EnemyOrientation.Right;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _enemyOrientation = EnemyOrientation.Left;
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
        if (_isAttacking)
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
        _wasPrevInSight = _enemyInSight;
        _distanceToEnemy = Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position);
        
        // Controllo le Y
        float enemyY = PlayerMovement.Instance.transform.position.y;
        float minY = Mathf.Min(transform.position.y - _enemyData.SightThresholdY, transform.position.y + _enemyData.SightThresholdY);
        float maxY = Mathf.Max(transform.position.y - _enemyData.SightThresholdY, transform.position.y + _enemyData.SightThresholdY); 

        // Controllo tutti i dati
        if (_distanceToEnemy < _enemyData.SightRadius && enemyY >= minY && enemyY <= maxY)
        {
            _enemyInSight = true;
        }
        else
        {
            _enemyInSight = false;
        }

        //
        if (_wasPrevInSight && !_enemyInSight)
        {
            if (_enemyOrientation == EnemyOrientation.Left)
            {
                _currentPoint = _pointB.transform;
                Flip();
            }
            else
            {
                _currentPoint = _pointA.transform;
                Flip();
            }
        }
    }

    void FollowEnemy()
    {
        // 
        if (!_enemyInSight)
            return;

        // 
        if (_isAttacking)
            return;

        //
        if (transform.position.x > PlayerMovement.Instance.transform.position.x)
        {
            if (_enemyOrientation == EnemyOrientation.Right)
            {
                Flip();
            }
            _rb.velocity = new Vector2(-_enemyData.RunSpeed, 0);
        }
        else
        {
            if (_enemyOrientation == EnemyOrientation.Left)
            {
                Flip();
            }
            _rb.velocity = new Vector2(_enemyData.RunSpeed, 0);
        }
    }

    void MeleeAttack()
    {
        //
        if (!_enemyInSight)
            return;

        //
        if (_isAttacking)
            return;

        // TODO: Controlla la y, 
        _distanceToEnemy = Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position);
        if (_distanceToEnemy <= _enemyData.MeleeRange)
        {
            _rb.velocity = Vector2.zero;

            // TODO: Eventualmente scegli tra gli attacchi, con un intero che cambia l'attacco nell'Animator
            _isAttacking = true;
        }
    }

    public void EnableAttackCollider()
    {
        _attackColliderGameObject.SetActive(true);

        // TODO: Passo allo script il danno
    }

    public void DisableAttackCollider()
    {
        _attackColliderGameObject.SetActive(false);
    }

    public void EndAttack()
    {
        //

        //
        Invoke(nameof(ReEnableAttack), _enemyData.DelayBetweenAttack);
    }

    private void ReEnableAttack()
    {
        _isAttacking = false;
    }

    private void CheckAggroDistance()
    {
        //
        if (!_enemyInSight)
            return;

        //
        if (_isAttacking)
            return;

        //
        float distance = Vector2.Distance(_startingPoint.transform.position, transform.position);
        if (distance >= _enemyData.AggroRange)
        {
            TeleportToStartPoint();
        }
    }

    private void TeleportToStartPoint()
    {
        //
        Instantiate(_smokeVFX, transform.position, Quaternion.identity);

        //
        _forceDisable = true;
        _rb.velocity = Vector2.zero;

        //
        Invoke(nameof(ResetToStartPosition), _enemyData.ResetTime);
    }

    private void ResetToStartPosition()
    {
        //
        transform.position = _startingPoint.transform.position;
        _enemyInSight = false;
        _wasPrevInSight = false;
        _isAttacking = false;
        _enemyOrientation = EnemyOrientation.Right;
        transform.localScale = new Vector3(1, 1, 1);
        _currentPoint = _pointB.transform;
        _forceDisable = false;
    }

    private void UpdateAnimator()
    {
        //
        _animator.SetFloat("XVelocity", Mathf.Abs(_rb.velocity.x));
        _animator.SetBool("EnemyInSight", _enemyInSight);
        _animator.SetBool("IsAttacking", _isAttacking);
    }

    private void Death()
    {
        // TODO: Disattiva collider, fai l'animazione, eventualmente dissolvi
        _animator.SetBool("Death", _isDeath);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _enemyData.AggroRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _enemyData.SightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyData.MeleeRange);

        //
        Gizmos.color = Color.white;
    }
}