using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item/Bonus")]
public class BonusItemData : ItemData
{
    [Header("Bonus")]
    [SerializeField] private BonusType _bonusType;
    [SerializeField] private float _bonusAmount;

    //
    public BonusType BonusType => _bonusType;
    public float BonusAmount => _bonusAmount;

    //
    public override ItemCategory Category => ItemCategory.Bonus;
}