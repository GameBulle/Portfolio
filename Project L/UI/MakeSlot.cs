using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSlot : MonoBehaviour
{
    [Header("Slot Row")]
    [Range(1, 10)]
    [SerializeField] int rowSlotCount = 1;

    [Header("Slot Column")]
    [Range(1, 10)]
    [SerializeField] int columnSlotCount = 1;

    [Header("Slot Margin")]
    [SerializeField] float slotMargin;

    [Header("Slot Size")]
    [SerializeField] float slotSize;

    [Header("ItemSlot Prefab")]
    [SerializeField] Slot slotPrefab;

    [Header("Content Area")]
    [SerializeField] RectTransform[] contentAreaRT;

    public enum SlotType { DropItemBox, Inventory, Quick, Equipment, SkillUI,SkillQuick,HolyRelic,Shop_Purchase,Shop_Sell,Shop_RePurchase, Reward}

    protected Slot[] slots;

    protected void MakeSlotInit(SlotType type, int column = 0)
    {
        gameObject.SetActive(true);

        RectTransform slotRect = slotPrefab.GetComponent<RectTransform>();
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        slots = new Slot[rowSlotCount * columnSlotCount * (column + 1)];
        if (column != 0)
            MakeSlotUI(type, column);
        else
            MakeSlotUI(type);
    }

    public void SetRowColumn(int row = 0,int column = 0)
    {
        if (row != 0)
            rowSlotCount = row;

        if (column != 0)
            columnSlotCount = column;
    }

    void MakeSlotUI(SlotType type)
    {
        Vector2 startPos = new Vector2(slotMargin, -slotMargin);
        Vector2 curPos = startPos;

        for (int i = 0; i < columnSlotCount; i++)
        {
            for (int j = 0; j < rowSlotCount; j++)
            {
                int slotIndex = (rowSlotCount * i) + j;

                RectTransform slotRT = MakeSlotPrefab();
                slotRT.pivot = new Vector2(0f, 1f);
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                Slot slotUI = slotRT.GetComponent<Slot>();
                slotUI.Index = slotIndex;
                slotUI.Initialize(type);
                slots[slotIndex] = slotUI;

                curPos.x += slotMargin + slotSize;
            }

            curPos.x = startPos.x;
            curPos.y -= slotMargin + slotSize;
        }
    }

    void MakeSlotUI(SlotType type,int column = 0)
    {
        for(int c = 0;c <= column; c++)
        {
            Vector2 startPos = new Vector2(slotMargin, -slotMargin);
            Vector2 curPos = startPos;

            for (int i = 0; i < columnSlotCount; i++)
            {
                for (int j = 0; j < rowSlotCount; j++)
                {
                    int slotIndex = (rowSlotCount * i) + j + (columnSlotCount * c);

                    RectTransform slotRT = MakeSlotPrefab(c);
                    slotRT.pivot = new Vector2(0f, 1f);
                    slotRT.anchoredPosition = curPos;
                    slotRT.gameObject.SetActive(true);
                    slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                    Slot slotUI = slotRT.GetComponent<Slot>();
                    slotUI.Index = slotIndex;
                    slotUI.Initialize(type);
                    slots[slotIndex] = slotUI;
                }

                curPos.x = startPos.x;
                curPos.y -= slotMargin * 2.5f + slotSize;
            }
        }
    }

    RectTransform MakeSlotPrefab(int index = 0)
    {
        Slot slot = Instantiate(slotPrefab);
        RectTransform rt = slot.GetComponent<RectTransform>();
        rt.SetParent(contentAreaRT[index]);
        return rt;
    }

    public int GetSlotCount() { return slots.Length;}
}
