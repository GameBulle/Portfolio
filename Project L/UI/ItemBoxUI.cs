using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ItemBoxUI : MakeSlot
{
    [SerializeField] TextMeshProUGUI goldText;

    ItemBox dropItemBox;
    int size;
    int gold;
    List<int> selectItemIndex = new List<int>();

    public void ClearDropItemBox()
    {
        for (int i = 0; i < size; i++)
            slots[i].ClearSlot();
        selectItemIndex.Clear();
    }

    public void Initialize(SlotType type)
    {
        MakeSlotInit(type);
    }

    public void LinkDropItemBox(ItemBox box)
    {
        dropItemBox = box;
        size = dropItemBox.GetDropItem().Count;
        SetSlots();
        gold = box.Gold;
        goldText.text = gold.ToString();
        gameObject.SetActive(true);
    }

    void SetSlots()
    {
        for (int i = 0; i < size; i++)
        {
            if (ItemMgr.Instance.GetItem(dropItemBox.GetDropItem()[i].id) as EquipableItemData)
                slots[i].SetSlot(ItemMgr.Instance.GetItem(dropItemBox.GetDropItem()[i].id), dropItemBox.GetDropItem()[i].amount);
            else
                slots[i].SetSlot(ItemMgr.Instance.GetItem(dropItemBox.GetDropItem()[i].id), dropItemBox.GetDropItem()[i].amount);
        }
            
    }

    public void ClickGet()
    {
        for(int i=0;i< size; i++)
        {
            if (slots[i].IsLeftClick)
            {
                selectItemIndex.Add(i);
                slots[i].ClearSlot();
            }  
        }

        goldText.text = "0";

        dropItemBox.ClickGet(selectItemIndex);
        selectItemIndex.Clear();
        SoundMgr.Instance.PlaySFXAudio("ItemGet");
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
