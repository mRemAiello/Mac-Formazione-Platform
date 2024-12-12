using System;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Data")]
    [SerializeField] private PlayerMovementData _playerData;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Ground")]    
    [SerializeField] private Transform _groundCheck; // Oggetto che verifica il contatto con il terreno  

    [Header("Attack")]
    [SerializeField] private Transform _attackColliderPosition;
    [SerializeField] private GameObject _attackCollider;

    //
    private PlayerStates _playerState = PlayerStates.Idle;
    private CharacterOrientation _playerOrientation;

    //
    private float _standardGravityScale;
    private int _jumpCount;
    private float _jumpTime;
    private float _moveInputHorizontal;
    private float _moveInputVertical;
    private bool _isSlowed = false;
    private float _slowJumpSpeed = 1;
    private float _slowSpeed = 1;
    private float _coyoteTimeCounter = 0;
    private float _jumpBufferCounter = 0;
    private GameObject _triggerMeleeAttackGameObject;

    //
    void Start()
    {
        _standardGravityScale = _rb.gravityScale;
    }

    void Update()
    {
        // Input orizzontale per il movimento (GetAxisRaw prende SOLO 0, 1, -1)
        // GetAxis prende anche valori intermedi (es. 0.01, -0.01)
        _moveInputHorizontal = Input.GetAxisRaw("Horizontal"); // Raccoglie l'input orizzontale (-1, 0, 1)
        _moveInputVertical = Input.GetAxisRaw("Vertical");

        // Muovi
        Move();
     
        //
        if (Input.GetKey(KeyCode.E))
        {
            Attack();
        }
    }

    private void LateUpdate()
    {
        // Controlla le condizioni di salto
        CheckJump();

        // Gravità
        ApplyCustomGravity();

        // Limita velocità
        ClampVelocity2D();

        // Ricontrolla le condizioni di salto
        ResetJump();

        //
        CheckDownButtonPressed();

        // Funzione che aggiorna l'animator
        UpdateAnimator();
    }

    private void Flip()
    {
        if (_playerOrientation == CharacterOrientation.Left)
        {
            transform.localScale = new Vector3(-1, 1, 1);  
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public bool IsGrounded()
    {
        // Controllo se il personaggio è a terra in base alla collisione e alla velocità verticale
        bool groundedByCollision = Physics2D.OverlapCircle(_groundCheck.position, _playerData.GroundCheckRadius, _playerData.GroundLayer);

        //
        // Debug.Log(groundedByCollision + " - " + Mathf.Abs(_rb.velocity.y));

        // Se è in contatto con il terreno e la velocità verticale è sufficientemente bassa, consideralo a terra
        if (groundedByCollision && Mathf.Abs(_rb.velocity.y) <= _playerData.VelocityThreshold)
        {
            return true;
        }

        return false;
    }

    private void Move()
    {
        if (_playerState == PlayerStates.Attack)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            return;
        }
        
        // Movimento del rigidbody impostando la velocità
        _rb.velocity = new Vector2(_moveInputHorizontal * _playerData.MoveSpeed * _slowSpeed, _rb.velocity.y);
        if (_rb.velocity.x > 0)
        {
            _playerOrientation = CharacterOrientation.Right;
            Flip();
        }
        else if (_rb.velocity.x < 0)
        {
            _playerOrientation = CharacterOrientation.Left;
            Flip();
        }
    }

    private void Attack()
    {
        if (_playerState == PlayerStates.Attack)
            return;

        // Attiva animator mio, attiva animator slash
        _triggerMeleeAttackGameObject = Instantiate(_attackCollider, _attackColliderPosition.position, Quaternion.identity);
        if (_playerOrientation == CharacterOrientation.Left)
        {
            _triggerMeleeAttackGameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

        // 
        TriggerMeleeAttack triggerMeleeAttack = _triggerMeleeAttackGameObject.GetComponent<TriggerMeleeAttack>();

        // TODO: Applicare i modificatori del danno
        triggerMeleeAttack?.Init(PlayerLifeManager.Instance, _playerData.AttackDamage);

        //
        ChangeState(PlayerStates.Attack);

        //
        Invoke(nameof(ResetAttackState), _playerData.AttackDelay);
    }

    private void ResetAttackState()
    {
        ChangeState(PlayerStates.Idle);

        //
        Destroy(_triggerMeleeAttackGameObject);
    }

    private void CheckJump()
    {
        // 
        if (_moveInputVertical < 0)
            return;

        // Coyote Time
        if (IsGrounded())
        {
            _coyoteTimeCounter = _playerData.CoyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffering
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _playerData.JumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        //
        if (_coyoteTimeCounter > 0 && _jumpBufferCounter > 0 && _playerState != PlayerStates.Attack)
        {
            //Debug.Log("Primo salto");
            ChangeState(PlayerStates.Jump);
            _rb.velocity = new Vector2(_rb.velocity.x, _playerData.JumpForce * _slowJumpSpeed);

            // Incrementa il numero di salti
            _jumpCount++;

            // 
            _jumpBufferCounter = 0;

            // TODO: Spawn del fumo, cambio animazione, suono
        }
        // Doppio salto
        else if (Input.GetButtonDown("Jump") && !IsGrounded() && _jumpCount < _playerData.MaxJumps && _playerState != PlayerStates.Attack)
        {
            //Debug.Log("Doppio salto");
            ChangeState(PlayerStates.Jump);
            _rb.velocity = new Vector2(_rb.velocity.x, _playerData.DoubleJumpForce * _slowJumpSpeed);

            // Incrementa il numero di salti
            _jumpCount++;
            _jumpTime = 0;

            // TODO: Spawn del fumo, cambio animazione, suono
        }

        // Coyote Time
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f && _jumpCount <= 1)
        {
            //Debug.Log("Coyote Time" + _jumpCount);
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            _coyoteTimeCounter = 0;
        }

        //
        if (_playerState == PlayerStates.Jump)
        {
            _jumpTime += Time.deltaTime;
        }
    }

    private void ResetJump()
    {
        // Resetta il contatore dei salti se il personaggio è a terra
        if (IsGrounded())
        {
            if (_playerState != PlayerStates.Attack)
                ChangeState(PlayerStates.Idle);
            
            // Resetta il numero di salti
            _jumpCount = 0;
            _jumpTime = 0;
        }
    }

    // Metodo per applicare la gravità manualmente
    private void ApplyCustomGravity()
    {
        // Aumento la gravità dopo un tot secondi di salto
        if (_playerState == PlayerStates.Jump && _jumpTime >= _playerData.JumpDelay)
        {
            _rb.gravityScale *= _playerData.GravityScale;
        }
        else
        {
            _rb.gravityScale = _standardGravityScale;
        }
    }

    // Controllo se ho premuto il tasto Giù + barra spaziatrice
    private void CheckDownButtonPressed()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump") && _moveInputVertical < 0)
        {
            Vector2 pos = transform.position;
            var distance = _playerData.PlatformDistanceToCheck;
            var layerMask = _playerData.PlatformDistanceLayerMask;
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.down, distance, layerMask);
            if (hits != null && hits.Length > 0)
            {
                Debug.Log($"Trovati {hits.Length} elementi.");
                foreach (var hit in hits)
                {
                    GameObject obj = hit.transform.gameObject;
                    DisableCollider disableScript = obj.GetComponent<DisableCollider>();
                    if (disableScript != null)
                    {
                        disableScript.DisablePlatformCollider();
                    }
                }
            }
        }
    }

    private void UpdateAnimator()
    {
        //
        _animator.SetFloat("XVelocity", Math.Abs(_rb.velocity.x));
        _animator.SetBool("IsJumping", _playerState == PlayerStates.Jump);
        _animator.SetBool("IsSlowed", _isSlowed);
    }

    private void ClampVelocity2D()
    {
        float clampedX = Mathf.Clamp(_rb.velocity.x, _playerData.MinXVelocity, _playerData.MaxXVelocity);
        float clampedY = Mathf.Clamp(_rb.velocity.y, _playerData.MinYVelocity, _playerData.MaxYVelocity);
        _rb.velocity = new Vector2(clampedX, clampedY);
    }

    public void Slow(float slowSpeed, float slowJumpSpeed)
    {
        _slowSpeed = slowSpeed;
        _slowJumpSpeed = slowJumpSpeed;
        _isSlowed = true;
    }

    public void RemoveSlow()
    {
        _slowSpeed = 1;
        _slowJumpSpeed = 1;
        _isSlowed = false;
    }

    public void ChangeState(PlayerStates state)
    {
        _playerState = state;
    }

    void OnDrawGizmos()
    {
        //
        if (_playerData == null)
            return;

        // Disegno Ground Check
        if (_groundCheck != null)
        {
            // Disegna il cerchio per visualizzare il controllo del terreno nell'Editor di Unity
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_groundCheck.position, _playerData.GroundCheckRadius);
            Gizmos.color = Color.white;
        }

        // Disegno Raycast
        if (_playerData != null)
        {
            Gizmos.color = Color.yellow;
            Vector2 to = (Vector2)transform.position + (Vector2.down * _playerData.PlatformDistanceToCheck);
            Gizmos.DrawLine(transform.position, to);
            Gizmos.color = Color.white;
        }
    }
}