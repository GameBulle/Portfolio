using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemSlot : MakeSlot
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemPrice;

    public int Index { get; set; }
    public bool HasItem { get; private set; }

    public void Initailize(SlotType type, ItemData itemData = null)
    {
        MakeSlotInit(type);
        slots[0].Index = this.Index;
        if (itemData != null)
        {
            slots[0].SetSlot(itemData);
            itemName.text = itemData.ItemName;
            itemPrice.text = itemData.BuyPrice.ToString();
        }
        else
        {
            itemName.text = null;
            itemPrice.text = null;
            HasItem = false;
        }
    }

    public void AddItem(int itemID, bool isSell)
    {
        ItemData item = ItemMgr.Instance.GetItem(itemID);
        slots[0].SetSlot(item);
        itemName.text = item.ItemName;
        if(isSell)
            itemPrice.text = item.SellPrice.ToString();
        else
            itemPrice.text = item.BuyPrice.ToString();
        HasItem = true;
    }

    public void ClearSlot()
    {
        slots[0].ClearSlot();
        itemName.text = null;
        itemPrice.text = null;
        HasItem = false;
    }

    public int GetItemID() { return slots[0].GetItemID(); }
}
