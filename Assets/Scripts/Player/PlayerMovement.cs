using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementData _playerMovementData;

    [Space]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    // Oggetto che verifica il contatto con il terreno    
    [SerializeField] private Transform _groundCheck;

    //
    private bool _isJumping = false;
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

        // Ricontrolla le condizioni di salto
        ResetJump();

        //
        CheckDownButtonPressed();

        // Funzione che aggiorna l'animator
        UpdateAnimator();
    }

    private void Flip()
    {
        // Flip della sprite in base alla direzione
        if (_moveInputHorizontal > 0)
        {
            _spriteRenderer.flipX = false; // Non capovolge la sprite
        }
        else if (_moveInputHorizontal < 0)
        {
            _spriteRenderer.flipX = true; // Capovolge la sprite
        }
    }

    public bool IsGrounded()
    {
        // Controllo se il personaggio è a terra in base alla collisione e alla velocità verticale
        bool groundedByCollision = Physics2D.OverlapCircle(_groundCheck.position, _playerMovementData.GroundCheckRadius, _playerMovementData.GroundLayer);

        //
        // Debug.Log(groundedByCollision + " - " + Mathf.Abs(_rb.velocity.y));

        // Se è in contatto con il terreno e la velocità verticale è sufficientemente bassa, consideralo a terra
        if (groundedByCollision && Mathf.Abs(_rb.velocity.y) <= _playerMovementData.VelocityThreshold)
        {
            return true;
        }

        return false;
    }

    private void Move()
    {
        // Movimento del rigidbody impostando la velocità
        _rb.velocity = new Vector2(_moveInputHorizontal * _playerMovementData.MoveSpeed * _slowSpeed, _rb.velocity.y);
    }

    private void CheckJump()
    {
        // 
        if (_moveInputVertical < 0)
            return;

        // Coyote Time
        if (IsGrounded())
        {
            _coyoteTimeCounter = _playerMovementData.CoyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffering
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _playerMovementData.JumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        //
        if (_coyoteTimeCounter > 0 && _jumpBufferCounter > 0)
        {
            _isJumping = true;
            _rb.velocity = new Vector2(_rb.velocity.x, _playerMovementData.JumpForce * _slowJumpSpeed);

            // Incrementa il numero di salti
            _jumpCount++;

            // 
            _jumpBufferCounter = 0;

            // TODO: Spawn del fumo, cambio animazione, suono
        }

        // Coyote Time
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _playerMovementData.JumpForce * _slowJumpSpeed * 0.5f);
            _coyoteTimeCounter = 0;
        }

        // Doppio salto        
        if (Input.GetButtonDown("Jump") && !IsGrounded() && _jumpCount < _playerMovementData.MaxJumps)
        {
            _isJumping = true;
            _rb.velocity = new Vector2(_rb.velocity.x, _playerMovementData.DoubleJumpForce * _slowJumpSpeed);

            // Incrementa il numero di salti
            _jumpCount++;
            _jumpTime = 0;

            // TODO: Spawn del fumo, cambio animazione, suono
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
        if (IsGrounded())
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
            _rb.gravityScale *= _playerMovementData.GravityScale;
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
            var distance = _playerMovementData.PlatformDistanceToCheck;
            var layerMask = _playerMovementData.PlatformDistanceLayerMask;
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
        _animator.SetBool("IsJumping", _isJumping);
        _animator.SetBool("IsSlowed", _isSlowed);
    }

    private void ClampVelocity2D()
    {
        float clampedX = Mathf.Clamp(_rb.velocity.x, _playerMovementData.MinXVelocity, _playerMovementData.MaxXVelocity);
        float clampedY = Mathf.Clamp(_rb.velocity.y, _playerMovementData.MinYVelocity, _playerMovementData.MaxYVelocity);
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

    void OnDrawGizmos()
    {
        // Disegno Ground Check
        if (_groundCheck != null)
        {
            // Disegna il cerchio per visualizzare il controllo del terreno nell'Editor di Unity
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_groundCheck.position, _playerMovementData.GroundCheckRadius);
            Gizmos.color = Color.white;
        }

        // Disegno Raycast
        if (_playerMovementData != null)
        {
            Gizmos.color = Color.yellow;
            Vector2 to = (Vector2)transform.position + (Vector2.down * _playerMovementData.PlatformDistanceToCheck);
            Gizmos.DrawLine(transform.position, to);
            Gizmos.color = Color.white;
        }
    }
}