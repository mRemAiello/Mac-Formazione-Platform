using UnityEngine;
using UnityEngine.UI;

public class PlayerRestoreHP : MagicBase
{
    public Button _button;

    void Start()
    {
        _button.onClick.AddListener(LaunchMagic);
    }

    void Update()
    {
        if (ManaData == null)
            return;

        if (!PlayerInventory.InstanceExists)
            return;

        //
        var item = PlayerInventory.Instance.Find(ManaData);
        if (Input.GetKeyDown(KeyCode.U) && item!= null && item.Amount >= ManaNeeded)
        {
            LaunchMagic();
        }
    }

    public void LaunchMagic()
    {
        PlayerController.Instance.RestoreHP();
        PlayerInventory.Instance.RemoveFromInventory(ManaData, ManaNeeded);
    }
}