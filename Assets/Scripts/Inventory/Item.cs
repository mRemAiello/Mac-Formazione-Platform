using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerInventory.Instance.AddToInventory(_itemData);
            
            // TODO: Inserire un'animazione
            // TODO: Disattivare il collider, lanciare un'animazione
            Destroy(this);
        }
    }

    // TODO: Animazione di "morte"
}