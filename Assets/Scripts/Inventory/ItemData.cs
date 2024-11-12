using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("Graphics")]
    [SerializeField] private Sprite _itemSprite;

    [Header("Display")]
    [SerializeField] private bool _isVisible = false;
    [SerializeField] private bool _showIfZero = true;
    [SerializeField] private bool _isStackable = true;

    [Header("Ranges")]
    [SerializeField, Range(1, 99999)] private int _max = 9999;

    //
    public Sprite Sprite => _itemSprite;
    public abstract ItemCategory Category { get; }
    public bool IsVisible => _isVisible;
    public bool ShowIfZero => _showIfZero;
    public bool IsStackable => _isStackable;
    public int Max => _max;
}