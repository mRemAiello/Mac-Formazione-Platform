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
    }

    private void Hide()
    {
        // TODO: Animazione
        _inventoryUI.SetActive(false);
    }
}