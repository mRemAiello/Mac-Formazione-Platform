using System;
using GamePix;
using GameUtils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController2 : Singleton<PlayerController2>, IDamageable
{
    [Header("Data")]
    [SerializeField] private PlayerMovementData _playerData;
    [SerializeField] private VoidEventAsset _onPlayerDamaged;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Ground")]
    [SerializeField] private Transform _groundCheck; // Oggetto che verifica il contatto con il terreno  

    [Header("Attack")]
    [SerializeField] private Transform _attackColliderPosition;
    [SerializeField] private GameObject _attackCollider;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private GameObject _audioSourcePrefab;

    [Header("Dead Animation")]
    [SerializeField] private float _deadAnimationTime;
    [SerializeField] private float _fadeAnimationTime;

    //
    private PlayerInputActions inputActions; // Riferimento alle Input Actions

    //
    private float _currentHP;
    public float maxHP;
    private float damagePerSecond = 0;
    private float seconds = 0;

    //
    public float CurrentHP => _currentHP;
    public bool IsDead => _currentHP <= 0;
    public bool IsAlive => _currentHP > 0;

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
    private bool _jumpButtonPressed = false;
    private bool _jumpButtonUp = true;
    private GameObject _triggerMeleeAttackGameObject;

    protected override void OnPostAwake()
    {
        inputActions = new PlayerInputActions();
    }

    //
    void Start()
    {
        _standardGravityScale = _rb.gravityScale;
        _currentHP = maxHP;
    }

    private void OnEnable()
    {
        inputActions.Enable(); // Abilita le Input Actions

        inputActions.Player.Move.performed += OnMovePressed; // Assegna il metodo OnMove all'azione Move
        inputActions.Player.Move.canceled += OnMoveReleased; // Assegna il metodo OnMoveCanceled all'azione Move
        inputActions.Player.Jump.performed += OnJumpPressed; // Assegna il metodo OnJump all'azione Jump
        inputActions.Player.Jump.canceled += OnJumpReleased; // Assegna il metodo OnJump all'azione Jump
        inputActions.Player.Attack.performed += OnAttack; // Assegna il metodo OnAttack all'azione Attack
    }

    private void OnDisable()
    {
        inputActions.Disable(); // Disabilita le Input Actions
    }

    private void LateUpdate()
    {
        //
        if (IsDead)
            return;

        // Controlla le condizioni di salto
        //Jump();

        //
        CheckMove();

        // Gravità
        ApplyCustomGravity();

        // Limita velocità
        ClampVelocity2D();

        // Ricontrolla le condizioni di salto
        ResetJump();

        // Funzione che aggiorna l'animator
        UpdateAnimator();
    }

    // Metodo per gestire il movimento
    private void OnMovePressed(InputAction.CallbackContext context)
    {
        // Legge l'input orizzontale
        Vector2 moveInput = context.ReadValue<Vector2>();
        _moveInputHorizontal = moveInput.x;
        _moveInputVertical = moveInput.y;
    }

    private void OnMoveReleased(InputAction.CallbackContext context)
    {
        _moveInputHorizontal = 0;
        _moveInputVertical = 0;
        _rb.linearVelocity = new Vector2(0, 0);
    }

    // Metodo per gestire il salto
    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        _jumpButtonUp = false;
        _jumpButtonPressed = true;
        Jump();
    }

    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        _jumpButtonUp = true;
        _jumpButtonPressed = false;
    }

    private void CheckMove()
    {
        //
        if (_playerState == PlayerStates.Attack)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            return;
        }

        // Movimento del rigidbody impostando la velocità
        _rb.linearVelocity = new Vector2(_moveInputHorizontal * _playerData.MoveSpeed * _slowSpeed, _rb.linearVelocity.y);
        if (_rb.linearVelocity.x > 0)
        {
            _playerOrientation = CharacterOrientation.Right;
            Flip();
        }
        else if (_rb.linearVelocity.x < 0)
        {
            _playerOrientation = CharacterOrientation.Left;
            Flip();
        }
    }

    private void Jump()
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
        if (_jumpButtonPressed)
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
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _playerData.JumpForce * _slowJumpSpeed);

            // Incrementa il numero di salti
            _jumpCount++;

            // 
            _jumpBufferCounter = 0;

            // TODO: Spawn del fumo, cambio animazione, suono
        }
        // Doppio salto
        else if (_jumpButtonPressed && !IsGrounded() && _jumpCount < _playerData.MaxJumps && _playerState != PlayerStates.Attack)
        {
            //Debug.Log("Doppio salto");
            ChangeState(PlayerStates.Jump);
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _playerData.DoubleJumpForce * _slowJumpSpeed);

            // Incrementa il numero di salti
            _jumpCount++;
            _jumpTime = 0;

            // TODO: Spawn del fumo, cambio animazione, suono
        }

        // Coyote Time
        if (_jumpButtonUp && _rb.linearVelocity.y > 0f && _jumpCount <= 1)
        {
            //Debug.Log("Coyote Time" + _jumpCount);
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.5f);
            _coyoteTimeCounter = 0;
        }

        //
        if (_playerState == PlayerStates.Jump)
        {
            _jumpTime += Time.deltaTime;
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
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
        triggerMeleeAttack?.Init(this, _playerData.AttackDamage);

        //
        GameObject audioSource = Instantiate(_audioSourcePrefab, _attackColliderPosition.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = _attackClip;
        audioSource.GetComponent<AudioSource>().Play();

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
        /*if (_playerState == PlayerStates.Jump && _jumpTime >= _playerData.JumpDelay)
        {
            _rb.gravityScale *= _playerData.GravityScale;
        }
        else
        {
            _rb.gravityScale = _standardGravityScale;
        }*/
    }

    private void UpdateAnimator()
    {
        //
        _animator.SetFloat("XVelocity", Math.Abs(_rb.linearVelocity.x));
        _animator.SetBool("IsJumping", _playerState == PlayerStates.Jump);
        _animator.SetBool("IsSlowed", _isSlowed);
    }

    private void ClampVelocity2D()
    {
        float clampedX = Mathf.Clamp(_rb.linearVelocity.x, _playerData.MinXVelocity, _playerData.MaxXVelocity);
        float clampedY = Mathf.Clamp(_rb.linearVelocity.y, _playerData.MinYVelocity, _playerData.MaxYVelocity);
        _rb.linearVelocity = new Vector2(clampedX, clampedY);
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

    public void AddDamagePerSecond(float damage, float seconds)
    {
        damagePerSecond = damage;
        this.seconds = seconds;
    }

    public void RemoveDamagePerSecond()
    {
        damagePerSecond = 0;
        seconds = 0;
    }

    public void TakeDamage(float damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            Death();
        }

        //
        _onPlayerDamaged?.Invoke();
    }

    public void RestoreHP()
    {
        _currentHP = maxHP;

        //
        _onPlayerDamaged?.Invoke();
    }

    private void Death()
    {
        _animator.SetBool("Dead", true);

        //
        Invoke(nameof(Fade), _deadAnimationTime);
    }

    private void Fade()
    {
        // Dissolvenza a nero
        FadeToBlack.Instance.StartFade();

        //
        Invoke(nameof(ShowAds), _fadeAnimationTime);
    }

    void ShowAds()
    {
        // show interstital ads
        Gpx.Ads.InterstitialAd(OnInterstitalAdSuccess);
    }

    [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    public static void OnInterstitalAdSuccess()
    {
        Instance.Respawn();
    }

    public void Respawn()
    {
        //
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
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
        if (groundedByCollision && Mathf.Abs(_rb.linearVelocity.y) <= _playerData.VelocityThreshold)
        {
            return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_groundCheck.position, _playerData.GroundCheckRadius);
            Gizmos.color = Color.white;
        }
    }
}