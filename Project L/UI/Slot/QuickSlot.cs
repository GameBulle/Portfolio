using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    Player player;
    DropItem[] items;
    int capacity;

    public QuickSlot(Player player, DropItem[] items = null)
    {
        this.player = player;
        InterfaceMgr.Instance.SetQuickSlot(this);
        capacity = InterfaceMgr.Instance.GetQuickSlotSize();
        if (items == null)
        {
            this.items = new DropItem[capacity];
            for (int i = 0; i < capacity; i++)
            {
                this.items[i].amount = 0;
                this.items[i].id = -1;
            }
        }
        else
        {
            this.items = items;
            for(int i=0;i<items.Length;i++)
            {
                if (this.items[i].id == -1)
                    continue;
                InterfaceMgr.Instance.AddItemToQuickSlotUI(this.items[i], i);
            }
        }
    }

    public void DragItemInventoryToQuick(DropItem item, int slotIndex)
    {
        items[slotIndex].id = item.id;
        items[slotIndex].amount = item.amount;
        InterfaceMgr.Instance.AddItemToQuickSlotUI(items[slotIndex],slotIndex);
    }

    public void DragItemQuickToInventory(int startIndex, int endIndex)
    {
        player.DragItemQuickToInventory(items[startIndex], endIndex);
        items[startIndex].id = -1;
        items[startIndex].amount = 0;
        InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Quick, startIndex);
    }

    public void DragItemQuickToQuick(int startIndex, int endIndex)
    {
        if (items[startIndex].id == items[endIndex].id)
        {
            CountableItemData cid = ItemMgr.Instance.GetItem(items[endIndex].id) as CountableItemData;
            if (items[endIndex].amount == cid.MaxAmount)
                ChangeItem(startIndex, endIndex);
            else if (items[startIndex].amount + items[endIndex].amount > cid.MaxAmount )
            {
                items[startIndex].amount -= cid.MaxAmount - items[endIndex].amount;
                items[endIndex].amount = cid.MaxAmount;
            }
            else
            {
                items[endIndex].amount += items[startIndex].amount;
                items[startIndex].amount = 0;
            }

            if (items[startIndex].amount <= 0)
                items[startIndex].id = -1;
        }
        else
            ChangeItem(startIndex, endIndex);

        if (items[startIndex].id == -1)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Quick, startIndex);
        else
            InterfaceMgr.Instance.AddItemToQuickSlotUI(items[startIndex], startIndex);

        if (items[endIndex].id == -1)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Quick, endIndex);
        else
            InterfaceMgr.Instance.AddItemToQuickSlotUI(items[endIndex], endIndex);

    }

    void ChangeItem(int startIndex, int endIndex)
    {
        DropItem temp = new();
        temp = items[startIndex];
        items[startIndex] = items[endIndex];
        items[endIndex] = temp;
    }

    public void Use(int slotIndex)
    {
        if (!InterfaceMgr.Instance.CheckQuickSlotItem(slotIndex))
            return;

        if (ItemMgr.Instance.GetItem(items[slotIndex].id).Use(player))
        {
            items[slotIndex].amount--;
            if (items[slotIndex].amount == 0)
            {
                InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Quick, slotIndex);
                items[slotIndex].id = 0;
                items[slotIndex].amount = 0;
            }
            else
                InterfaceMgr.Instance.AddItemToQuickSlotUI(items[slotIndex], slotIndex);
        }
    }

    public void SplitItem(int slotIndex, int amount = 0)
    {
        if(amount == 0)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Quick, slotIndex);
        else
        {
            if (items[slotIndex].amount <= 0)
                InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Quick, slotIndex);
            else
                InterfaceMgr.Instance.AddItemToQuickSlotUI(items[slotIndex], slotIndex);
        }
    }

    public void DeleteItem(int itemIndex)
    {
        items[itemIndex].amount = 0;
        items[itemIndex].id = -1;
    }

    public DropItem[] GetAllItem() { return items; }
    public DropItem GetItem(int slotIndex) { return items[slotIndex]; }
}
