using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Droppable
{
    [SerializeField] private Item _item;
    [SerializeField] private float _probability;
}