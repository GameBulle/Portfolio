using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemQuickSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Drag")]
    [SerializeField] Image dragIcon;
    [SerializeField] TextMeshProUGUI dragAmountText;
    [SerializeField] GameObject dragObject;

    [SerializeField] TextMeshProUGUI amountText;

    [Header("Quick Number")]
    [SerializeField] TextMeshProUGUI quickNumber;

    bool startDrag;

    public override void Initialize(MakeSlot.SlotType type)
    {
        base.Initialize(type);

        startDrag = false;

        quickNumber.gameObject.SetActive(true);
        quickNumber.text = (Index + 1).ToString();

        amountText.gameObject.SetActive(false);
        dragObject.SetActive(false);
        quickNumber.gameObject.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HasItem)
        {
            dragObject.SetActive(true);
            startDrag = true;
            dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("QuickSlot UI").transform);
            GameObject.FindGameObjectWithTag("QuickSlot UI").GetComponent<Canvas>().sortingOrder = 10;

            InterfaceMgr.Instance.SetDragStartIndex(slotType, Index);
        }
    }

    public override void SetSlot(ItemData itemData, int amount)
    {
        this.icon.sprite = itemData.Icon;
        dragIcon.sprite = itemData.Icon;

        if (itemData.Type == ItemData.ItemType.Countable)
        {
            amountText.gameObject.SetActive(true);
            dragAmountText.gameObject.SetActive(true);

            amountText.text = amount.ToString();
            dragAmountText.text = amount.ToString();
        }
        this.icon.gameObject.SetActive(true);
    }

    public override void ClearSlot()
    {
        icon.sprite = null;
        amountText.text = null;

        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
        dragObject.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!startDrag)
            return;
        dragObject.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InterfaceMgr.Instance.SetDragEndInfo(slotType, Index, HasItem);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!startDrag)
            return;

        dragObject.transform.SetParent(parentTr);
        dragObject.transform.localPosition = Vector3.zero;

        if (!IsOverUI())
            InterfaceMgr.Instance.ClearSlot(slotType, Index);
        else
            InterfaceMgr.Instance.DragItem();

        startDrag = false;
        dragObject.SetActive(false);
        GameObject.FindGameObjectWithTag("QuickSlot UI").GetComponent<Canvas>().sortingOrder = 2;
    }

    private bool IsOverUI()
=> EventSystem.current.IsPointerOverGameObject();
}
