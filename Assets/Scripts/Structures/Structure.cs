using System;
using UnityEngine;

public class Structure : MonoBehaviour
{
    //
    public GameObject UpgradeUI;

    //
    public StructureData Data;

    void Start()
    {
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

    public void HideUpgrade()
    {
        // TODO: Animazione
        UpgradeUI.SetActive(false);
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
    }
}