using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillQuickSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Drag")]
    [SerializeField] Image dragIcon;
    [SerializeField] GameObject dragObject;

    [Header("Quick Number")]
    [SerializeField] TextMeshProUGUI quickNumber;

    bool startDrag;

    public override void Initialize(MakeSlot.SlotType type)
    {
        base.Initialize(type);

        startDrag = false;

        dragObject.SetActive(false);
        quickNumber.gameObject.SetActive(true);

        switch (Index)
        {
            case 0:
                quickNumber.text = "Z";
                break;
            case 1:
                quickNumber.text = "X";
                break;
            case 2:
                quickNumber.text = "C";
                break;
        }
    }

    public override void SetSlot(SkillData skillData)
    {
        this.icon.sprite = skillData.Icon;
        dragIcon.sprite = skillData.Icon;

        this.icon.gameObject.SetActive(true);
    }

    public override void ClearSlot()
    {
        icon.sprite = null;

        icon.gameObject.SetActive(false);
        dragObject.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HasItem)
        {
            dragObject.SetActive(true);
            startDrag = true;
            dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("QuickSlot UI").transform);
            //GameObject.FindGameObjectWithTag("Inventory UI").GetComponent<Canvas>().sortingOrder = 10;

            InterfaceMgr.Instance.SetDragStartIndex(slotType, Index);
        }
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
        {
            ClearSlot();
            InterfaceMgr.Instance.InitDragInfo();
        }
        else
            InterfaceMgr.Instance.DragSkill();

        startDrag = false;
        dragObject.SetActive(false);
    }

    private bool IsOverUI()
    => EventSystem.current.IsPointerOverGameObject();
}
