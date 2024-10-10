using UnityEngine;

public class PlayerFallDamage : MonoBehaviour
{
    [SerializeField] private PlayerLife _playerLife;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float _fallThreshold = 40f;
    [SerializeField] private float _damage;
    [SerializeField] private float _damageMultiplier = 1;

    private float _firstY;
    private float _lastY;
    private bool _isFalling = false;

    private void LateUpdate()
    {
        //
        if (_playerMovement.IsGrounded && _isFalling)
        {
            _lastY = _firstY - transform.position.y;
             Debug.Log("Last Y: " + _lastY);
            _isFalling = false;

            //
            float difference = Mathf.Abs(_firstY - _lastY);
            Debug.Log(difference);
            if (difference > _fallThreshold)
            {
                float damage = _damage + ((difference - _fallThreshold) * _damageMultiplier);
                _playerLife.TakeDamage(damage);
            }
        }

        // Controllare Is Grounded
        if (!_playerMovement.IsGrounded && !_isFalling)
        {
            _firstY = transform.position.y;
            Debug.Log("First Y: " + _firstY);
            _isFalling = true;
        }
    }
}