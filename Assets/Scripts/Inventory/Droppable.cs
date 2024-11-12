using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Droppable
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _probability;

    //
    public GameObject ItemPrefab => _itemPrefab;
    public float Probability => _probability;
}