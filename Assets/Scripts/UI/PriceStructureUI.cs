using TMPro;
using UnityEditor;
using UnityEngine;

public class PriceStructureUI : MonoBehaviour
{
    public TextMeshPro UpgradeText;
    public string StructureText = "Press X to Upgrade ({0})";
    // TODO: Eventualmente si può anche mettere un testo per quando non c'è l'upgrade
    // public string NoUpgradeText = "No Upgrade Available";
    public string RepairText = "Press X to Repair ({0})";


    public void ShowUpgradeText(int price)
    {
        // TODO: Capire la piattaforma e selezionare la giusta sprite
        // string str = "<sprite name=x-xbox>";
        UpgradeText.text = string.Format(StructureText, price);
    }

    public void ShowRepairText(int price)
    {
        UpgradeText.text = string.Format(RepairText, price);
    }
}