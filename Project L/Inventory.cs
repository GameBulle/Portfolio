using System;
using UnityEngine;
using static ItemData;

public class Inventory : MonoBehaviour
{
    int remainAmount;
    int gold;
    int capacity;
    Player player;
    bool nextItem;
    DropItem[] items;
    CountableItemData cid = null;

    public Inventory(Player player, DropItem[] items = null, int gold = 500) 
    { 
        this.gold = gold; 
        this.player = player;
        capacity = InterfaceMgr.Instance.GetInventorySize();
        InterfaceMgr.Instance.SetInventory(this);

        if(items == null)
        {
            this.items = new DropItem[capacity];
            for (int i = 0; i < capacity; i++)
            {
                this.items[i].id = -1;
                this.items[i].amount = 0;
            }
        }
        else
        {
            this.items = items;
            for(int i=0;i<capacity;i++)
            {
                if (this.items[i].id == -1)
                    continue;
                InterfaceMgr.Instance.AddItemToInventoryUI(this.items[i], i);
            }             
        }
        
        nextItem = false;
        InterfaceMgr.Instance.UpdateGold(gold);
    }

    public void GetItem(DropItem dropItems)
    {
        ItemData.ItemType type = ItemMgr.Instance.GetItem(dropItems.id).Type;

        if (type == ItemData.ItemType.Countable)
        {
            nextItem = false;
            remainAmount = dropItems.amount;
            cid = null;

            CheckGetItemIsCountable(dropItems.id);

            if (nextItem)
                return;
        }


        for (int i = 0; i < capacity; i++)
        {
            if (items[i].amount == 0)
            {
                if (type == ItemData.ItemType.Countable)
                {
                    if (cid != null)
                    {
                        items[i].id = dropItems.id;
                        if (remainAmount > cid.MaxAmount)
                        {
                            remainAmount -= (cid.MaxAmount - items[i].amount);
                            items[i].amount = cid.MaxAmount;
                            InterfaceMgr.Instance.AddItemToInventoryUI(items[i], i);
                        }
                        else
                        {
                            items[i].amount = remainAmount;
                            InterfaceMgr.Instance.AddItemToInventoryUI(items[i], i);
                            break;
                        }
                    }

                    items[i] = dropItems;
                    InterfaceMgr.Instance.AddItemToInventoryUI(items[i], i);
                    break;
                }
                else
                {
                    items[i] = dropItems;
                    InterfaceMgr.Instance.AddItemToInventoryUI(items[i], i);
                    break;
                }
            }
        }
    }

    public void PlusGold(int gold)
    {
        this.gold += gold;
        InterfaceMgr.Instance.UpdateGold(this.gold);
    }

    public bool MinusGold(int gold)
    {
        if(this.gold >= gold)
        {
            this.gold -= gold;
            InterfaceMgr.Instance.UpdateGold(this.gold);
            return true;
        }
        return false;
    }

    void CheckGetItemIsCountable(int itemID)
    {
        bool check = false;
        cid = ItemMgr.Instance.GetItem(itemID) as CountableItemData;
        int findIndex = Array.FindIndex(items, a => a.id == itemID);

        while (findIndex != -1)
        {
            if (items[findIndex].amount < cid.MaxAmount)
            {
                check = (items[findIndex].amount + remainAmount) > cid.MaxAmount ? true : false;
                if (check)
                {
                    remainAmount -= (cid.MaxAmount - items[findIndex].amount);
                    items[findIndex].amount = cid.MaxAmount;
                    InterfaceMgr.Instance.AddItemToInventoryUI(items[findIndex], findIndex);
                }
                else
                {
                    items[findIndex].amount += remainAmount;
                    remainAmount = 0;
                    nextItem = true;
                    InterfaceMgr.Instance.AddItemToInventoryUI(items[findIndex], findIndex);
                    break;
                }
            }

            findIndex = Array.FindIndex(items, findIndex + 1, a => a.id == itemID);
        }
    }

    public void Use(MakeSlot.SlotType slotType, int index)
    {
        switch (ItemMgr.Instance.GetItem(items[index].id).Type)
        {
            case ItemType.Countable:
                if (ItemMgr.Instance.GetItem(items[index].id).Use(player))
                {
                    items[index].amount--;
                    if (items[index].amount == 0)
                    {
                        InterfaceMgr.Instance.ClearSlot(slotType, index);
                        items[index].id = 0;
                        items[index].amount = 0;
                    }
                    else
                        InterfaceMgr.Instance.AddItemToInventoryUI(items[index], index);
                }
                break;
            case ItemType.Equipable:
                EquipableItemData eid = ItemMgr.Instance.GetItem(items[index].id) as EquipableItemData;
                int unequipItem = player.EquipmentExchange(eid.EquipType);
                if (unequipItem != -1)
                {
                    items[index].id = unequipItem;
                    items[index].amount = 1;
                    InterfaceMgr.Instance.AddItemToInventoryUI(items[index], index);
                }
                else
                {
                    items[index].id = 0;
                    items[index].amount = 0;
                    InterfaceMgr.Instance.ClearSlot(slotType, index);
                }
                eid.Use(player);
                break;
        }
    }

    bool CheckCountableItem(int index1, int index2)
    {
        return ((items[index1].id == items[index2].id) && (ItemMgr.Instance.GetItem(items[index1].id).Type == ItemData.ItemType.Countable));
    }

    public void DragItemInventoryToInventory(int startIndex, int endIndex, int amount = 0)
    {
        if (amount != 0)
        {
            items[startIndex].amount -= amount;
            items[endIndex].id = items[startIndex].id;
            items[endIndex].amount += amount;
        }
        else
        {
            if (CheckCountableItem(startIndex, endIndex))
            {
                CountableItemData cid = ItemMgr.Instance.GetItem(items[endIndex].id) as CountableItemData;
                if (items[endIndex].amount == cid.MaxAmount)
                    ChangeItem(startIndex, endIndex);
                else if (items[endIndex].amount + items[startIndex].amount > cid.MaxAmount)
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
        }

        if (items[startIndex].id == -1)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Inventory, startIndex);
        else
            InterfaceMgr.Instance.AddItemToInventoryUI(items[startIndex], startIndex);

        if (items[endIndex].id == -1)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Inventory, endIndex);
        else
            InterfaceMgr.Instance.AddItemToInventoryUI(items[endIndex], endIndex);
    }

    void ChangeItem(int startIndex, int endIndex)
    {
        DropItem temp = new();
        temp = items[startIndex];
        items[startIndex] = items[endIndex];
        items[endIndex] = temp;
    }

    public void DragItemInventoryToQuick(int startIndex, int endIndex, int amount = 0)
    {
        if (ItemMgr.Instance.GetItem(items[startIndex].id).Type == ItemData.ItemType.Countable)
            player.DragItemInventoryToQuickSlot(items[startIndex], startIndex, endIndex, amount);
    }

    public void DragItemQuickToInventory(DropItem item, int slotIndex)
    {
        if (item.id == -1)
            return;

        remainAmount = item.amount;
        CheckGetItemIsCountable(item.id);

        items[slotIndex].id = item.id;
        items[slotIndex].amount = remainAmount;
        InterfaceMgr.Instance.AddItemToInventoryUI(items[slotIndex], slotIndex);
    }

    public void SplitItem(int itemIndex, int amount = 0)
    {
        if (amount == 0)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Inventory, itemIndex);
        else
        {
            items[itemIndex].amount -= amount;
            if (items[itemIndex].amount <= 0)
                InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Inventory, itemIndex);
            else
                InterfaceMgr.Instance.AddItemToInventoryUI(items[itemIndex], itemIndex);
        }
    }

    public void DeleteItem(int itemIndex)
    {
        items[itemIndex].amount = 0;
        items[itemIndex].id = -1;
    }

    public void SellItem(int index)
    {
        items[index].amount--;
        if (items[index].amount <= 0)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Inventory, index);
        else
            InterfaceMgr.Instance.AddItemToInventoryUI(items[index], index);
    }

    public void Sort()
    {
        for (int i = 0; i < items.Length - 1; i++) 
        {
            if(items[i].id == -1)
            {
                for (int j = i + 1; j < items.Length; j++)
                {
                    if(items[j].id != -1)
                    {
                        ChangeItem(i, j);
                        InterfaceMgr.Instance.AddItemToInventoryUI(items[i], i);
                        InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Inventory, j);
                        break;
                    }
                }
            }
        }
    }

    public int GetGold() { return gold; }
    public DropItem[] GetInventoryAllItem() { return items; }
}
