using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private int _itemsToDrop = 1;
    [SerializeField] private List<Droppable> _drops;

    public void DropItems()
    {
        if (_drops == null || _drops.Count <= 0)
        {
            Debug.Log("Inserisci un drop " + name);
            return;
        }

        int itemDropped = 0;
        foreach (Droppable droppable in _drops)
        {
            int number = Random.Range(0, 100);
            Debug.Log("Generato " + number);
            if (number <= droppable.Probability)
            {
                GameObject obj = Instantiate(droppable.Data.Prefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                // TODO: Mettere una velocità x casuale, y casuale, spawnare ogni tot secondi

                //
                itemDropped++;
            }

            //
            if (itemDropped >= _itemsToDrop)
            {
                break;
            }
        }
    }
}