using UnityEngine;

// TODO: Ease DotTween
// https://i.redd.it/mutfwze2koz41.gif

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    [Space]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private float _velocityThreshold = 0.1f;

    [Space]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _collider;

    //
    private bool _isAnimationFinished = false;
    private bool _isItemPicked = false;

    void LateUpdate()
    {
        if (_isAnimationFinished)
        {
            return;
        }

        if (IsGrounded())
        {
            _rb.velocity = Vector3.zero;
            Destroy(_rb);
            _collider.isTrigger = true;

            //
            _isAnimationFinished = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isItemPicked)
        {
            return;
        }

        Debug.Log("Chiamata");
        Debug.Log(other.gameObject);
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerInventory.Instance.AddToInventory(_itemData);

            //
            _isItemPicked = true;

            // TODO: lanciare Coroutine

            // TODO: Fare animazione
            Destroy(gameObject);

            // TODO: Suono
        }
    }

    private bool IsGrounded()
    {
        // Controllo se il personaggio è a terra in base alla collisione e alla velocità verticale
        bool groundedByCollision = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        // Se è in contatto con il terreno e la velocità verticale è sufficientemente bassa, consideralo a terra
        if (groundedByCollision && Mathf.Abs(_rb.velocity.y) <= _velocityThreshold)
        {
            return true;
        }

        return false;
    }

    // TODO: Animazione di "morte"
}