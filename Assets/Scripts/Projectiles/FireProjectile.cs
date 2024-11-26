using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;

    //
    private Transform _enemyToSeek;
    private bool _seeking;

    void Update()
    {
        if (!_seeking)
            return;

        //
        transform.position = Vector3.Lerp(transform.position, _enemyToSeek.transform.position, _speed * Time.deltaTime);   
    }

    // TODO: Aggiungi il danno

    public void Seek(Transform enemy)
    {
        if (_seeking)
            return;

        //
        _enemyToSeek = enemy;
        _seeking = true;
    }
}