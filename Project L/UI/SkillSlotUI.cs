using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotUI : MakeSlot
{
    SkillSlot skillSlot;

    public int Size => slots.Length;

    public void SetSkillSlot(SkillSlot slot) { this.skillSlot = slot; }
    public void SetSkill(int id, int index)
    {
        if (id == -1)
            return;
        slots[index].SetSlot(SkillMgr.Instance.GetSkill(id)); 
    }

    public void Initialize(SlotType type)
    {
        MakeSlotInit(type);
        
        gameObject.SetActive(true);
    }

    public void ClearSlot(int slotIndex) { slots[slotIndex].ClearSlot(); }

    public void DragSkillUIToQuick(int id, int index)
    {
        skillSlot.DragSkillUIToQuick(id, index);
        SetSkill(id, index);
    }

    public void DragQuickToQuick(int startIndex,int endIndex)
    {
        if (startIndex == endIndex)
            return;
        skillSlot.DragQuickToQuick(startIndex, endIndex);
        slots[startIndex].ClearSlot();
        slots[endIndex].SetSlot(SkillMgr.Instance.GetSkill(skillSlot.GetSkillID(endIndex)));
    }

    public void InitSkills()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].ClearSlot();
        skillSlot.InitSkills();
    }
}
