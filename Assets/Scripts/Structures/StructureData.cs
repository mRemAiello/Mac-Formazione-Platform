using UnityEngine;

[CreateAssetMenu(fileName = "StructureData", menuName = "ScriptableObjects/StructureData", order = 1)]
public class StructureData : ScriptableObject
{
    public Sprite NormalStructure;
    public int Health;
    public int Defence;
    public int Attack;
    public int AttackRange;
    
    [Space]
    public bool IsFirstStructure = false;

    [Header("Repair")]
    public GameObject DestroyedParticle;
    public Sprite DamagedSprite;
    public int ManaToRepair;

    [Header("Upgrades")]
    public int ManaToUpgrade;
    public GameObject Upgrade;
}