using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che gestisce l'inventario generico (scrigni, nemici, boss, giocatore, ecc)

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> _items;

    public void AddToInventory(ItemData item, int amount = 1)
    {
        // TODO: Verificare se l'oggetto già c'è (stackable)
        _items.Add(item);
    }
}