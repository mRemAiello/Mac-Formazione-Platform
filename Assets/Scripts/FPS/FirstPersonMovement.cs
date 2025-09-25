using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float x;
    private float z;
    private PlayerInputActions inputActions;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnDisable()
    {
        inputActions.Disable(); // Disabilita le Input Actions
    }

    private void OnEnable()
    {
        inputActions.Enable(); // Abilita le Input Actions
        // OnMove() -> OnSound() -> OnParticle()
        inputActions.Player.Move.performed += OnMove;


        /*inputActions.Player.Move.performed += OnMovePressed; // Assegna il metodo OnMove all'azione Move
        inputActions.Player.Move.canceled += OnMoveReleased; // Assegna il metodo OnMoveCanceled all'azione Move
        inputActions.Player.Jump.performed += OnJumpPressed; // Assegna il metodo OnJump all'azione Jump
        inputActions.Player.Jump.canceled += OnJumpReleased; // Assegna il metodo OnJump all'azione Jump
        inputActions.Player.Attack.performed += OnAttack; // Assegna il metodo OnAttack all'azione Attack
        inputActions.Player.Upgrade.performed += OnUpgradePressed;*/
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        x = value.x;
        z = value.y;
    }



    /*public void OnMove(InputValue inputValue)
    {
        //    Avanti
        // Sx         Dx
        //   Indietro
        Vector2 value = inputValue.Get<Vector2>();
        x = value.x;
        z = value.y;
    }*/

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // (x, 0, z) * Time.deltaTime * move = (x, 0, z)
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(speed * Time.deltaTime * move);

        // (0, y, 0)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}