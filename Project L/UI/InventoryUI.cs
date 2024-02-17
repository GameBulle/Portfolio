using TMPro;
using UnityEngine;

public class InventoryUI : MakeSlot
{
    [SerializeField] TextMeshProUGUI goldText;
    Inventory inventory;

    public int Size => slots.Length;

    public void SetInventory(Inventory inventory)   { this.inventory = inventory; }

    public void Initialize(SlotType type)
    {
        MakeSlotInit(type);
        gameObject.SetActive(false);
    }

    public void AddItemToInventoryUI(DropItem item, int index)
    {
        slots[index].SetSlot(ItemMgr.Instance.GetItem(item.id), item.amount);
    }

    public void Use(int index)
    {
        inventory.Use(SlotType.Inventory, index);
    }

    public void ClearSlot(int index)
    {
        slots[index].ClearSlot();
        inventory.DeleteItem(index);
    }

    public void DragItemInventoryToInventory(int startDragSlotIndex, int endDragSlotIndex, int amount = 0)
    {
        if (startDragSlotIndex == endDragSlotIndex)
            return;
        inventory.DragItemInventoryToInventory(startDragSlotIndex, endDragSlotIndex, amount);
    }

    public void DragItemInventoryToQuickSlot(int startDragSlotIndex, int endDragSlotIndex, int amount = 0)
    {
        inventory.DragItemInventoryToQuick(startDragSlotIndex, endDragSlotIndex, amount);
    }

    public void DeleteItem(int slotIndex, int amount)
    {
        inventory.SplitItem(slotIndex, amount);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SellItem(int index)
    {
        inventory.SellItem(index);
    }

    public void Sort() { inventory.Sort(); }

    public void UpdateGold(int gold) { goldText.text = gold.ToString(); }

    private void OnEnable()
    {
        SoundMgr.Instance.PlaySFXAudio("UI_Open");
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
