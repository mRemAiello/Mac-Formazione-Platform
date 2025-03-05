using UnityEngine;

public class Structure : MonoBehaviour, IDamageable
{
    [Header("Structure")]
    public int CurrentHealth;
    public StructureData Data;

    public SpriteRenderer SpriteRenderer;
    public Collider2D StructureCollider;

    [Header("UI")]
    public GameObject UpgradeUI;

    //
    public bool IsFirstStructure => Data.IsFirstStructure;
    public bool IsDead => CurrentHealth <= 0;
    public bool IsAlive => CurrentHealth > 0;

    void Start()
    {
        CurrentHealth = Data.Health;

        //
        UpgradeUI.SetActive(false);
    }

    public void Upgrade()
    {
        if (Data.Upgrade != null)
        {
            var upgradedStructure = Instantiate(Data.Upgrade, transform.position, transform.rotation);
            upgradedStructure.transform.SetParent(transform.parent);
            Destroy(gameObject);
        }
    }

    public void Repair()
    {
        CurrentHealth = Data.Health;
        StructureCollider.isTrigger = false;
        //SpriteRenderer.sprite = Data.NormalStructure;
        // TODO: Cambia sprite in quella "normale"
    }

    public void HideUpgrade()
    {
        // TODO: Animazione
        UpgradeUI.SetActive(false);
    }

    public void ShowRepair()
    {
        //

        //
        UpgradeUI.SetActive(true);

        //
        var priceStructureUI = UpgradeUI.GetComponent<PriceStructureUI>();
        priceStructureUI.ShowRepairText(Data.ManaToRepair);
    }

    public void ShowUpgrade()
    {
        if (Data.Upgrade == null)
        {
            UpgradeUI.SetActive(false);
            return;
        }

        // TODO: Animazione
        UpgradeUI.SetActive(true);

        //
        var priceStructureUI = UpgradeUI.GetComponent<PriceStructureUI>();
        priceStructureUI.ShowUpgradeText(Data.ManaToUpgrade);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        //
        CurrentHealth -= (int)damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }
    }

    private void Death()
    {
        StructureCollider.isTrigger = true;

        // TODO: Cambia sprite in quella danneggiata
        // TODO: Instanzia particelle di distruzione
        //SpriteRenderer.sprite = Data.DamagedSprite;
    }
}