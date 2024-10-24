using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    // TODO: ID
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private ItemCategory _itemCategory;
    [SerializeField] private bool _isVisible = true;

    //
    public GameObject Prefab => _itemPrefab;
    public Sprite Sprite => _itemSprite;
    public ItemCategory Category => _itemCategory;
    public bool IsVisible => _isVisible;
}