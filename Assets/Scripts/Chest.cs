using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // TODO: Sistemare
    [SerializeField] private Drop _drop;

    void OnCollisionEnter2D(Collision2D other)
    {
        _drop.DropItems();

        // TODO: Quando la funzione viene richiamata, disattivare il collider, disabilitare lo script
        // TODO: Inoltre, salvare lo stato
    }
}
