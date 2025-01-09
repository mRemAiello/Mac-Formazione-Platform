using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float destroyTime;

    void Start()
    {
        Invoke(nameof(Destroy), destroyTime);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}