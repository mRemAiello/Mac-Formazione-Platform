using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollider : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _timeToReactiveCollider;

    public void DisablePlatformCollider()
    {
        _collider.enabled = false;

        //
        Invoke(nameof(EnableCollider), _timeToReactiveCollider);
    }

    private void EnableCollider()
    {
        _collider.enabled = true;
    }
}