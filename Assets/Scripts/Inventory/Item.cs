using UnityEngine;

// TODO: Ease DotTween
// https://i.redd.it/mutfwze2koz41.gif

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    [Space]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _particleEffect;

    //
    public Collider2D Collider => _collider;

    //
    private bool _isItemPicked = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isItemPicked)
        {
            return;
        }

        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerInventory.Instance.AddToInventory(_itemData);

            //
            _isItemPicked = true;

            //
            Instantiate(_particleEffect, transform.position, Quaternion.identity);
            
            // TODO: Suono

            //
            Destroy(gameObject);
        }
    }
}