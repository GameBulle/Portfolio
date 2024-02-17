using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MakeSlot
{
    QuickSlot quickSlot;

    public int Size => slots.Length;

    public void SetQuickSlot(QuickSlot quickSlot) { this.quickSlot = quickSlot; }

    public void Initialize(SlotType type)
    {
        MakeSlotInit(type);
        gameObject.SetActive(true);
    }

    public void ClearSlot(int slotIndex)
    { 
        slots[slotIndex].ClearSlot();
        quickSlot.DeleteItem(slotIndex);
    }

    public void AddItemToQuickSlot(DropItem item, int slotIndex)
    {
        slots[slotIndex].SetSlot(ItemMgr.Instance.GetItem(item.id), item.amount);
    }
    
    public void DragItemQuickToInventory(int startIndex, int endIndex)
    {
        quickSlot.DragItemQuickToInventory(startIndex, endIndex);
    }

    public void DragItemQuickToQuick(int startIndex, int endIndex)
    {
        if (startIndex == endIndex)
            return;
        quickSlot.DragItemQuickToQuick(startIndex, endIndex);
    }

    public void DeleteItem(int slotIndex, int amount)
    {
        quickSlot.SplitItem(slotIndex, amount);
    }

    public bool CheckSlot(int index) { return slots[index].HasItem; }
}
