using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItemAmount : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private SingleItemUI _singleItemUI;

    void Start()
    {
        _singleItemUI.SetSprite(_itemData.Sprite);
        _singleItemUI.SetItemNumber("0");
    }

    void Update()
    {
        if (!PlayerInventory.InstanceExists)
        {
            return;
        }

        //
        ItemWithAmount itemWithAmount = PlayerInventory.Instance.Find(_itemData);
        if (itemWithAmount != null)
        {
            _singleItemUI.SetItemNumber(itemWithAmount.Amount);
        }
    }
}