using System;

[Serializable]
public class ItemWithAmount
{
    public ItemData ItemData;
    public int Amount;

    public ItemWithAmount(ItemData itemData, int amount)
    {
        ItemData = itemData;
        Amount = amount;
    }
}