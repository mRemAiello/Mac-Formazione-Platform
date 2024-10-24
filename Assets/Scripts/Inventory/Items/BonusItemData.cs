using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item/Bonus")]
public class BonusItemData : ItemData
{
    [SerializeField] private BonusType _bonusType;
    [SerializeField] private float _amount;
}