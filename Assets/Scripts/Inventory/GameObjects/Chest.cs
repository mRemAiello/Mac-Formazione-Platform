using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // TODO: Sistemare
    [SerializeField] private Drop _drop;

    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;

    //
    private bool _isOpened = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_isOpened)
        {
            return;
        }

        //
        _isOpened = true;

        //
        _collider.enabled = false;

        // TODO: Animazione
        //_animator.SetBool("Opened", true);

        //
        _drop.DropItems();

        // TODO: Quando la funzione viene richiamata, disattivare il collider, disabilitare lo script
        // TODO: Inoltre, salvare lo stato
    }
}
