using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Droppable
{
    [SerializeField] private ItemData _item;
    [SerializeField] private float _probability;

    //
    public ItemData Data => _item;
    public float Probability => _probability;
}