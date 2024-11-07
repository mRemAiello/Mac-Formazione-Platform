using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : Singleton<InventoryUIManager>
{
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private List<GameObject> _gridItemList;

    //
    private bool _active = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_active)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    private void Show()
    {
        // TODO: Animazione
        _inventoryUI.SetActive(true);

        //
        _active = true;

        // Nascondo all'inizio gli oggetti della griglia
        foreach (GameObject obj in _gridItemList)
        {
            SingleItemUI singleItemUI = obj.GetComponent<SingleItemUI>();
            if (singleItemUI != null)
            {
                singleItemUI.HideItem();
            }
        }

        //
        var items = PlayerInventory.Instance.Items;
        int i = 0;
        foreach (var item in items)
        {
            if (item.ItemData.IsVisible)
            {
                SingleItemUI singleItemUI = _gridItemList[i].GetComponent<SingleItemUI>();
                if (singleItemUI != null)
                {
                    if ((item.Amount == 0 && item.ItemData.ShowIfZero) || item.Amount > 0)
                    {
                        singleItemUI.SetSprite(item.ItemData.Sprite);
                        singleItemUI.SetItemNumber(item.Amount);
                        i++;
                    }
                }
            }
        }
    }

    private void Hide()
    {
        // TODO: Animazione
        _inventoryUI.SetActive(false);

        //
        _active = false;
    }
}