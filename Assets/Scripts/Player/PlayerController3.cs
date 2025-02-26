using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController3 : Singleton<PlayerController3>
{
    [Header("Data")]
    [SerializeField] private PlayerMovementData _playerData;
    [SerializeField] private Rigidbody2D _rb;
    public TextMeshProUGUI _moneyText;
    public int money;

    //
    public Structure currentStructure;

    //
    private PlayerInputActions inputActions; // Riferimento alle Input Actions
    private float _moveInputHorizontal;
    private float _moveInputVertical;

    protected override void OnPostAwake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable(); // Abilita le Input Actions

        inputActions.Player.Move.performed += OnMovePressed; // Assegna il metodo OnMove all'azione Move
        inputActions.Player.Move.canceled += OnMoveReleased; // Assegna il metodo OnMoveCanceled all'azione Move

        inputActions.Player.Upgrade.performed += OnUpgradePressed;
    }

    private void OnUpgradePressed(InputAction.CallbackContext context)
    {
        // TODO: Verifica di avere i soldi (condizioni per l'upgrade)
        if (currentStructure != null && money >= currentStructure.Data.MoneyToUpgrade)
        {
            currentStructure.Upgrade();
            money -= currentStructure.Data.MoneyToUpgrade;
        }
    }

    void LateUpdate()
    {
        //
        CheckMove();

        //
        CheckStructureNearby();

        //
        if (_moneyText != null)
        {
            _moneyText.text = money.ToString();
        }
    }

    private void CheckStructureNearby()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _playerData.InteractionRange);
        bool foundStructure = false;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Structure>(out var structure))
            {
                currentStructure = structure;
                currentStructure.ShowUpgrade();
                foundStructure = true;
            }
        }

        //
        if (!foundStructure)
        {
            if (currentStructure != null)
            {
                currentStructure.HideUpgrade();
                currentStructure = null;
            }
        }
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
        //_rb.linearVelocity = new Vector2(0, 0);
    }

    private void CheckMove()
    {
        // Movimento del rigidbody impostando la velocità
        Vector2 newPosition = transform.position;
        newPosition.x += _moveInputHorizontal * _playerData.MoveSpeed * Time.deltaTime;
        newPosition.y += _moveInputVertical * _playerData.MoveSpeed * Time.deltaTime;
        _rb.MovePosition(newPosition);
        //_rb.linearVelocity = new Vector2(_moveInputHorizontal * _playerData.MoveSpeed * _slowSpeed, _rb.linearVelocity.y);

        //
        /*if (_playerState == PlayerStates.Attack)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            return;
        }


        if (_rb.linearVelocity.x > 0)
        {
            _playerOrientation = CharacterOrientation.Right;
            Flip();
        }
        else if (_rb.linearVelocity.x < 0)
        {
            _playerOrientation = CharacterOrientation.Left;
            Flip();
        }*/
    }
}