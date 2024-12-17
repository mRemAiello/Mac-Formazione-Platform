using UnityEngine;

public class MagicBase : MonoBehaviour
{
    [SerializeField] private ItemData _manaData;
    [SerializeField] private int _manaNeeded;

    //
    public ItemData ManaData => _manaData;
    public int ManaNeeded => _manaNeeded;
}