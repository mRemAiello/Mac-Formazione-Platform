using System;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : GroundChecker
{
    [SerializeField] private PlayerMovementData _playerMovementData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    
    //
    private bool _isJumping = false;
    private float _standardGravityScale;
    private int _jumpCount;
    private float _jumpTime;
    private float _moveInput;
    private bool _isSlowed = false;
    private float _slowJumpSpeed = 1;
    private float _slowSpeed = 1;

    void Start()
    {
        _standardGravityScale = rb.gravityScale;
    }

    void Update()
    {
        // Input orizzontale per il movimento (GetAxisRaw prende SOLO 0, 1, -1)
        // GetAxis prende anche valori intermedi (es. 0.01, -0.01)
        _moveInput = Input.GetAxisRaw("Horizontal"); // Raccoglie l'input orizzontale (-1, 0, 1)

        // Muovi
        Move();
    }

    private void LateUpdate()
    {
        // Controlla le condizioni di salto
        CheckJump();
        
        // Gravità
        ApplyCustomGravity();

        // Limita velocità
        ClampVelocity2D();

        // Flip Sprite
        Flip();

        //
        CheckIfIsGrounded();

        // Ricontrolla le condizioni di salto
        ResetJump();

        // Funzione che aggiorna l'animator
        UpdateAnimator();
    }

    private void Flip()
    {
        // Flip della sprite in base alla direzione
        if (_moveInput > 0)
        {
            _spriteRenderer.flipX = false; // Non capovolge la sprite
        }
        else if (_moveInput < 0)
        {
            _spriteRenderer.flipX = true; // Capovolge la sprite
        }
    }

    private void Move()
    {
        // Movimento del rigidbody impostando la velocità
        rb.velocity = new Vector2(_moveInput * _playerMovementData.MoveSpeed * _slowSpeed, rb.velocity.y);
    }

    private void CheckJump()
    {
        //
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded)
            {
                _isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, _playerMovementData.JumpForce * _slowJumpSpeed);

                // Incrementa il numero di salti
                _jumpCount++;

                // TODO: Spawn del fumo, cambio animazione, suono
            }
            // Doppio salto        
            else if (!IsGrounded && _jumpCount < _playerMovementData.MaxJumps)
            {
                _isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, _playerMovementData.DoubleJumpForce * _slowJumpSpeed);

                // Incrementa il numero di salti
                _jumpCount++;
                _jumpTime = 0;

                // TODO: Spawn del fumo, cambio animazione, suono
            }
        }

        //
        if (_isJumping)
        {
            _jumpTime += Time.deltaTime;
        }
    }

    private void ResetJump()
    {
        // Resetta il contatore dei salti se il personaggio è a terra
        if (IsGrounded)
        {
            _isJumping = false;
            // Resetta il numero di salti
            _jumpCount = 0;
            _jumpTime = 0;
        }
    }

    // Metodo per applicare la gravità manualmente
    private void ApplyCustomGravity()
    {
        // Aumento la gravità dopo un tot secondi di salto
        if (_isJumping && _jumpTime >= _playerMovementData.JumpDelay) 
        {
            rb.gravityScale *= _playerMovementData.GravityScale;
        }
        else
        {
            rb.gravityScale = _standardGravityScale;
        }
    }

    private void UpdateAnimator()
    {
        //
        _animator.SetFloat("XVelocity", Math.Abs(rb.velocity.x));
        _animator.SetBool("IsJumping", _isJumping);
        _animator.SetBool("IsSlowed", _isSlowed);
    }

    private void ClampVelocity2D()
    {
        float clampedX = Mathf.Clamp(rb.velocity.x, _playerMovementData.MinXVelocity, _playerMovementData.MaxXVelocity);
        float clampedY = Mathf.Clamp(rb.velocity.y, _playerMovementData.MinYVelocity, _playerMovementData.MaxYVelocity);
        rb.velocity = new Vector2(clampedX, clampedY);
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
}