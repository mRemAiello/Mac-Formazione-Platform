using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Animation")]
    [SerializeField] private float _timeBetweenSpawn = 0.4f;
    [SerializeField] private float _firstAnimationDuration = 0;
    [SerializeField] private Ease _firstAnimationEase;
    [SerializeField] private float _finalAnimationDuration = 0;
    [SerializeField] private Ease _finalAnimationEase;
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
        Vector3 newPos = transform.position + new Vector3(x, y, 0);

        //
        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0, obj.transform.DOLocalMove(newPos, _firstAnimationDuration)).SetEase(_firstAnimationEase);
        sequence.OnComplete(() => { OnAnimationCompleted(obj, newPos); });
    }

    private void OnAnimationCompleted(GameObject obj, Vector3 newPos)
    {
        if (obj.TryGetComponent<Collider2D>(out var collider))
        {
            //
            collider.enabled = true;
            collider.isTrigger = true;

            //
            RaycastHit2D raycastHits = Physics2D.Raycast(newPos, Vector2.down, Mathf.Infinity, _groundLayer);
            Vector3 finalPos = raycastHits.transform.position;
            finalPos.x = newPos.x;
            finalPos.y += raycastHits.collider.bounds.size.y / 2;
            finalPos.y += collider.bounds.size.y / 2;
            finalPos.z = 0;
      
            //
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, obj.transform.DOLocalMove(finalPos, _finalAnimationDuration)).SetEase(_finalAnimationEase);
        }
    }
}