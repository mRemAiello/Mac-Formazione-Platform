using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour, IDamageable, IKnockable
{
    [SerializeField] protected EnemyData _enemyData;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _enemyCollider;
    [SerializeField] private Animator _animator;

    [Header("Patrol")]
    [SerializeField] private GameObject _startingPoint;
    [SerializeField] private GameObject _pointA;
    [SerializeField] private GameObject _pointB;

    [Header("Melee Attack")]
    [SerializeField] private GameObject _attackColliderGameObject;

    [Header("UI")]
    [SerializeField] private Slider _redSlider;
    [SerializeField] private Slider _yellowSlider;
    [SerializeField] private float _redFillSpeed;
    [SerializeField] private float _yellowHpFillSpeed;

    [Header("VFX")]
    [SerializeField] private GameObject _smokeVFX;

    //
    private float _currentHP;
    private float _maxHP;

    //
    private CharacterOrientation _enemyOrientation = CharacterOrientation.Right;
    private Transform _currentPoint;
    private bool _wasPrevInSight = false;
    private bool _enemyInSight = false;
    private bool _enemyInMeleeRange = false;
    private bool _knocked = false;
    private bool _isDeath = false;
    private bool _forceDisable = false;

    //
    public Rigidbody2D InternalRigidbody => _rb;
    public EnemyData InternalEnemyData => _enemyData;
    public bool IsDead => _isDeath;
    public bool IsAlive => _currentHP > 0;
    public bool IsKnockable => _enemyData.IsKnockable;
    public float KnockBackForce => _enemyData.KnockBackForce;
    public float KnockBackTime => _enemyData.KnockBackTime;
    public float StunTime => _enemyData.StunTime;
    protected bool IsAttacking { get; set; }
    protected bool IsRangedAttacking { get; set; }
    protected bool EnemyInSight => _enemyInSight;
    protected float DistanceToEnemy { get; set; }

    //
    void Start()
    {
        _currentPoint = _pointB.transform;

        //
        _currentHP = _enemyData.HP;
        _maxHP = _enemyData.HP;

        //
        _redSlider.minValue = 0;
        _yellowSlider.minValue = 0;
        _redSlider.maxValue = _maxHP;
        _yellowSlider.maxValue = _maxHP;
        _redSlider.value = _currentHP;
        _yellowSlider.value = _currentHP;

        //
        OnPostStart();
    }

    void Update()
    {
        //
        if (_isDeath)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        //
        if (_forceDisable)
            return;

        // 
        if (_knocked)
            return;

        //
        Patrol();

        //
        CheckEnemyInSight();

        // 
        FollowEnemy();

        //  
        MeleeAttack();

        //
        RangedAttack();

        //
        CheckAggroDistance();
    }

    void LateUpdate()
    {
        //
        UpdateHP();

        //
        UpdateAnimator();
    }

    private void Flip()
    {
        if (_enemyOrientation == CharacterOrientation.Left)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _enemyOrientation = CharacterOrientation.Right;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _enemyOrientation = CharacterOrientation.Left;
        }
    }

    protected void Patrol()
    {
        // 
        if (!_enemyData.CanPatrol)
            return;

        //
        if (_enemyInSight)
            return;

        //
        if (IsAttacking)
            return;

        //
        if (_currentPoint == null)
            return;

        // TODO: Controllare sotto di lui se c'è un burrone
        if (_currentPoint == _pointB.transform)
        {
            _rb.linearVelocity = new Vector2(_enemyData.WalkSpeed, 0);
        }
        else
        {
            _rb.linearVelocity = new Vector2(-_enemyData.WalkSpeed, 0);
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

    protected void Turn()
    {
        //
        _rb.linearVelocity = Vector2.zero;

        //
        float timeToLaunch = Random.Range(_enemyData.TurnAnimationDurationMin, _enemyData.TurnAnimationDurationMax);

        //
        Invoke(nameof(EndTurn), timeToLaunch);
    }

    protected void EndTurn()
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

    protected virtual void CheckEnemyInSight()
    {
        if (!PlayerController.InstanceExists)
            return;

        //
        _wasPrevInSight = _enemyInSight;
        DistanceToEnemy = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        // Controllo le Y
        float enemyY = PlayerController.Instance.transform.position.y;
        float minY = Mathf.Min(transform.position.y - _enemyData.SightThresholdY, transform.position.y + _enemyData.SightThresholdY);
        float maxY = Mathf.Max(transform.position.y - _enemyData.SightThresholdY, transform.position.y + _enemyData.SightThresholdY);

        // Controllo tutti i dati
        if (DistanceToEnemy < _enemyData.SightRadius && enemyY >= minY && enemyY <= maxY)
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
            if (_enemyOrientation == CharacterOrientation.Left)
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

    protected void FollowEnemy()
    {
        // 
        if (!_enemyInSight)
            return;

        // 
        if (IsAttacking)
            return;

        //
        if (IsRangedAttacking)
            return;

        //
        if (!_enemyData.CanFollow)
            return;

        //
        if (transform.position.x > PlayerController.Instance.transform.position.x)
        {
            if (_enemyOrientation == CharacterOrientation.Right)
            {
                Flip();
            }
            _rb.linearVelocity = new Vector2(-_enemyData.RunSpeed, 0);
        }
        else
        {
            if (_enemyOrientation == CharacterOrientation.Left)
            {
                Flip();
            }
            _rb.linearVelocity = new Vector2(_enemyData.RunSpeed, 0);
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

    public void ReEnableAttack()
    {
        IsAttacking = false;
    }

    private void CheckAggroDistance()
    {
        //
        if (!_enemyInSight)
            return;

        //
        if (IsAttacking)
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
        _rb.linearVelocity = Vector2.zero;

        //
        Invoke(nameof(ResetToStartPosition), _enemyData.ResetTime);
    }

    private void ResetToStartPosition()
    {
        //
        transform.position = _startingPoint.transform.position;
        _enemyInSight = false;
        _wasPrevInSight = false;
        IsAttacking = false;
        IsRangedAttacking = false;
        _enemyOrientation = CharacterOrientation.Right;
        transform.localScale = new Vector3(1, 1, 1);
        _currentPoint = _pointB.transform;
        _forceDisable = false;
    }

    private void UpdateAnimator()
    {
        //
        _animator.SetFloat("XVelocity", Mathf.Abs(_rb.linearVelocity.x));
        _animator.SetBool("EnemyInSight", _enemyInSight);
        _animator.SetBool("IsAttacking", IsAttacking);
        _animator.SetBool("IsRangedAttacking", IsRangedAttacking);
    }

    private void Death()
    {
        //
        StopAllCoroutines();

        //
        _isDeath = true;
        IsAttacking = false;
        IsRangedAttacking = false;
        _knocked = false;
        _enemyInSight = false;
        _wasPrevInSight = false;
        _currentPoint = null;

        //
        _animator.SetBool("IsAttacking", false);
        _animator.SetBool("IsRangedAttacking", false);
        _animator.SetBool("Death", _isDeath);

        // Disattivo collider e rigidbody
        _rb.linearVelocity = Vector3.zero;
        _rb.isKinematic = true;
        _enemyCollider.enabled = false;

        //
        OnPostDeath();
    }

    public void TakeDamage(float damage)
    {
        _currentHP -= damage;

        //
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            Death();
        }
    }

    public void KnockBack(Transform enemyTransform, float knockBackForce, float knockBackTime, float stunForce)
    {
        _knocked = true;
        IsAttacking = false;

        //
        StartCoroutine(StunTimer(knockBackTime, stunForce));

        //
        Vector2 direction = (transform.position - enemyTransform.position).normalized;
        _rb.linearVelocity = direction * knockBackForce;
    }

    private IEnumerator StunTimer(float knockBackTime, float stunForce)
    {
        yield return new WaitForSeconds(knockBackTime);

        //
        _rb.linearVelocity = Vector3.zero;

        //
        yield return new WaitForSeconds(stunForce);

        //
        _knocked = false;
    }

    private void UpdateHP()
    {
        //
        float target = Mathf.Lerp(_redSlider.value, _currentHP, _redFillSpeed * Time.deltaTime);
        float targetEffect = Mathf.Lerp(_yellowSlider.value, _currentHP, _yellowHpFillSpeed * Time.deltaTime);

        //
        _redSlider.value = target;
        _yellowSlider.value = targetEffect;
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
        if (_enemyData != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _enemyData.AggroRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _enemyData.SightRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _enemyData.MeleeRange);
        }

        //
        Gizmos.color = Color.white;

        //
        OnPostDrawGizmos();
    }

    //
    public abstract void OnPostStart();
    public abstract void MeleeAttack();
    public abstract void RangedAttack();
    public abstract void EndRangedAttack();
    public abstract void OnPostDeath();
    public abstract void OnPostDrawGizmos();
}