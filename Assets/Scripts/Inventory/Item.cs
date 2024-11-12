using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// TODO: Ease DotTween
// https://i.redd.it/mutfwze2koz41.gif

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField, Range(1, 99999)] private int _amount = 1;

    [Space]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _particleEffect;

    //
    public Collider2D Collider => _collider;

    //
    private bool _isItemPicked = false;
    private List<Sequence> _sequences;

    public void AddSequence(Sequence sequence)
    {
        if (_sequences == null)
        {
            _sequences = new List<Sequence>();
        }

        //
        _sequences.Add(sequence);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isItemPicked)
        {
            return;
        }

        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerInventory.Instance.AddToInventory(_itemData, _amount);

            //
            _isItemPicked = true;

            //
            GameObject particle = Instantiate(_particleEffect);
            particle.transform.SetParent(PlayerMovement.Instance.transform);
            particle.transform.localPosition = Vector3.zero;
            particle.transform.localRotation = Quaternion.identity;
            
            // TODO: Suono

            //
            foreach (Sequence sequence in _sequences)
            {
                sequence?.Kill();
            }

            //
            Destroy(gameObject);
        }
    }
}