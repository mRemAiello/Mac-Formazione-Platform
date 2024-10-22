using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    [Space]
    [SerializeField] private List<Droppable> _drops;
}