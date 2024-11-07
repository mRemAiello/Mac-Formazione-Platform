using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item/Key Item")]
public class KeyItemData : ItemData
{
    //
    public override ItemCategory Category => ItemCategory.KeyItem;
}