using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che gestisce l'inventario generico (scrigni, nemici, boss, giocatore, ecc)

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemWithAmount> _items = new List<ItemWithAmount>();

    //
    public List<ItemWithAmount> Items => _items;

    void Start()
    {
        _items = new List<ItemWithAmount>();
    }

    public ItemWithAmount Find(ItemData itemData)
    {
        foreach (ItemWithAmount itemWithAmount in _items)
        {
            if (itemWithAmount.ItemData == itemData)
            {
                return itemWithAmount;
            }
        }

        return null;
    }

    public int IndexOf(ItemData itemData)
    {
        int i = 0;
        foreach (ItemWithAmount itemWithAmount in _items)
        {
            if (itemWithAmount.ItemData == itemData)
            {
                return i;
            }
            i++;
        }

        return -1;
    }

    public void AddToInventory(ItemData item, int amount = 1)
    {
        ItemWithAmount itemWithAmount = Find(item);
        if (itemWithAmount == null)
        {
            itemWithAmount = new ItemWithAmount(item, amount);
            _items.Add(itemWithAmount);
        }
        else
        {
            if (item.IsStackable)
            {
                itemWithAmount.Amount += amount;
            }
            else
            {
                itemWithAmount = new ItemWithAmount(item, amount);
                _items.Add(itemWithAmount);
            }
        }

        //
        itemWithAmount.Amount = Mathf.Min(itemWithAmount.Amount, item.Max);
    }

    public void RemoveFromInventory(ItemData item, int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("Amount must be greater than 0.");
            return;
        }

        ItemWithAmount itemWithAmount = Find(item);
        if (itemWithAmount != null)
        {
            int index = IndexOf(item);
            if (index != -1)
            {
                _items[index].Amount -= amount;
                if (_items[index].Amount < 0)
                {
                    _items[index].Amount = 0;
                }
            }
        }
    }
}