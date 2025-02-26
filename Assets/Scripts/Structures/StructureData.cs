using UnityEngine;

[CreateAssetMenu(fileName = "StructureData", menuName = "ScriptableObjects/StructureData", order = 1)]
public class StructureData : ScriptableObject
{
    public int Health;
    public int Defence;
    public int Attack;
    public int AttackRange;

    [Header("Upgrades")]
    public int MoneyToUpgrade;
    public GameObject Upgrade;
}