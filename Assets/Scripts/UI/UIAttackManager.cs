using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAttackManager : MonoBehaviour
{
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _jumpButton;
    private Image _image;

    private void Awake()
    {
        //_attackButton.onClick.AddListener(OnAttackButtonClicked);
        _jumpButton.onClick.AddListener(OnJumpButtonClicked);
    }

    private void OnJumpButtonClicked()
    {
        if (PlayerController.InstanceExists)
        {
            PlayerController.Instance.JumpButtonPressed();
        }
    }

    private void OnAttackButtonClicked()
    {
        if (PlayerController.InstanceExists)
        {
            PlayerController.Instance.Attack();
        }
    }
}