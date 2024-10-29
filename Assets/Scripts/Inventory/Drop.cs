using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float _timeBetweenSpawn = 0.4f;
    [SerializeField] private float _minForceX = 0;
    [SerializeField] private float _maxForceX = 0;
    [SerializeField] private float _minForceY = 0;
    [SerializeField] private float _maxForceY = 0;

    [Header("Drops")]
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
                StartCoroutine(SpawnItem(droppable, itemDropped, itemDropped * _timeBetweenSpawn));

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

    private IEnumerator SpawnItem(Droppable droppable, int itemIndex, float time)
    {
        yield return new WaitForSeconds(time);

        //
        GameObject obj = Instantiate(droppable.Data.Prefab, transform.position, Quaternion.identity);
        
        //
        float x;
        float y = Random.Range(_minForceY, _maxForceY);
        if (itemIndex % 2 == 0)
        {
            x = -Random.Range(_minForceX, _maxForceX);
        }
        else
        {
            x = Random.Range(_minForceX, _maxForceX);
        }

        //
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(x, y);

        // TODO: Mettere una velocità x casuale, y casuale, spawnare ogni tot secondi
    }
}