using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

struct DragInfo
{
    public MakeSlot.SlotType slotyType;
    public int slotIndex;
    public bool hasItem;
}

public class Draggable : MonoBehaviour
{
    [Header("Draggable UI")]
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] QuickSlotUI quickSlotUI;
    [SerializeField] SkillSlotUI skillSlotUI;

    DragInfo startDrag;
    DragInfo endDrag;

    public void Initialize()
    {
        gameObject.SetActive(true);
        inventoryUI.Initialize(MakeSlot.SlotType.Inventory);
        quickSlotUI.Initialize(MakeSlot.SlotType.Quick);
        skillSlotUI.Initialize(MakeSlot.SlotType.SkillQuick);
    }

    public void SetSkill(int id, int index) { skillSlotUI.SetSkill(id, index); }

    public void InventoryUI()
    {
        if(inventoryUI.gameObject.activeSelf == true)
            inventoryUI.gameObject.SetActive(false);
        else
            inventoryUI.gameObject.SetActive(true);

    }


    public int GetInventorySize()
    {
        return inventoryUI.Size;
    }

    public int GetQuickSlotSize() {  return quickSlotUI.Size; }
    public int GetSkillSlotSize() { return skillSlotUI.Size; }

    public void AddItemInventoryUI(DropItem item, int index)
    {
        inventoryUI.AddItemToInventoryUI(item, index);
    }

    public void AddItemQuickSlotUI(DropItem item, int slotIndex)
    {
        quickSlotUI.AddItemToQuickSlot(item, slotIndex);
    }

    public void GetUseItemSlotIndex(int index)
    {
        inventoryUI.Use(index);
    }

    public void ClearSlot(MakeSlot.SlotType slotType, int index)
    {
        switch(slotType)
        {
            case MakeSlot.SlotType.Inventory:
                inventoryUI.ClearSlot(index);
                break;
            case MakeSlot.SlotType.Quick:
                quickSlotUI.ClearSlot(index);
                break;
        }
    }

    public void SetInventory(Inventory inventory) { inventoryUI.SetInventory(inventory); }
    public void SetQuickSlot(QuickSlot quickSlot) { quickSlotUI.SetQuickSlot(quickSlot); }
    public void SetSkillSlot(SkillSlot skillSlot) { skillSlotUI.SetSkillSlot(skillSlot); }

    public void SetDragStartInfo(MakeSlot.SlotType slotType, int slotIndex)
    {
        startDrag.slotyType = slotType;
        startDrag.slotIndex = slotIndex;
    }

    public void SetDragEndInfo(MakeSlot.SlotType slotType, int slotIndex, bool hasItem)
    {
        endDrag.slotyType = slotType;
        endDrag.slotIndex = slotIndex;
        endDrag.hasItem = hasItem;
    }

    public void DragItem()
    {
        if(endDrag.slotIndex != -1)
        {
            switch (startDrag.slotyType)
            {
                case MakeSlot.SlotType.Inventory:
                    switch (endDrag.slotyType)
                    {
                        case MakeSlot.SlotType.Inventory:
                            inventoryUI.DragItemInventoryToInventory(startDrag.slotIndex, endDrag.slotIndex);
                            break;
                        case MakeSlot.SlotType.Quick:
                            inventoryUI.DragItemInventoryToQuickSlot(startDrag.slotIndex, endDrag.slotIndex);
                            break;
                    }
                    break;
                case MakeSlot.SlotType.Quick:
                    switch (endDrag.slotyType)
                    {
                        case MakeSlot.SlotType.Inventory:
                            quickSlotUI.DragItemQuickToInventory(startDrag.slotIndex, endDrag.slotIndex);
                            break;
                        case MakeSlot.SlotType.Quick:
                            quickSlotUI.DragItemQuickToQuick(startDrag.slotIndex, endDrag.slotIndex);
                            break;
                    }
                    break;
            }
        }
        ClearDragInfo();
    }

    public void DragSkill()
    {
        if(endDrag.slotIndex != -1)
        {
            switch(startDrag.slotyType)
            {
                case MakeSlot.SlotType.SkillUI:
                    skillSlotUI.DragSkillUIToQuick(startDrag.slotIndex, endDrag.slotIndex);
                    break;
                case MakeSlot.SlotType.SkillQuick:
                    skillSlotUI.DragQuickToQuick(startDrag.slotIndex,endDrag.slotIndex);
                    break;
            }
        }
    }

    public void InitDragInfo()
    {
        ClearDragInfo();
    }

    void ClearDragInfo()
    {
        startDrag.slotIndex = -1;
        endDrag.slotIndex = -1;
        endDrag.hasItem = false;
    }

    public bool CheckQuickSlotItem(int index) { return quickSlotUI.CheckSlot(index); }

    public void InputAmount(MakeSlot.SlotType slotType, int slotIndex, int amount)
    {
        if (endDrag.slotIndex == -1)
        {
            switch (slotType)
            {
                case MakeSlot.SlotType.Inventory:
                    inventoryUI.DeleteItem(slotIndex, amount);
                    break;
                case MakeSlot.SlotType.Quick:
                    quickSlotUI.DeleteItem(slotIndex, amount);
                    break;
            }
        }
        else if(!endDrag.hasItem)
        {
            switch(endDrag.slotyType)
            {
                case MakeSlot.SlotType.Inventory:
                    inventoryUI.DragItemInventoryToInventory(startDrag.slotIndex, endDrag.slotIndex, amount);
                    break;
                case MakeSlot.SlotType.Quick:
                    Debug.Log("inventory to quick");
                    inventoryUI.DragItemInventoryToQuickSlot(startDrag.slotIndex, endDrag.slotIndex, amount);
                    break;
            }
        }
        ClearDragInfo();
    }

    public void InitSkills()
    {
        skillSlotUI.InitSkills();
    }

    public void SellItem(int index)
    {
        inventoryUI.SellItem(index);
    }

    public void UpdateGold(int gold) { inventoryUI.UpdateGold(gold); }
}
