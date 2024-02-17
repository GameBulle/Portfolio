using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : Slot, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    [Header("Drag")]
    [SerializeField] Image dragIcon;
    [SerializeField] GameObject dragObject;

    [Header("ToolTip")]
    [SerializeField] GameObject toolTipObject;
    [SerializeField] TextMeshProUGUI toolTipText;
    [SerializeField] TextMeshProUGUI itemName;

    float toolTipBgHeight;

    public override void Initialize(MakeSlot.SlotType type)
    {
        base.Initialize(type);

        RectTransform rectTr = toolTipObject.GetComponent<RectTransform>();
        toolTipBgHeight = rectTr.sizeDelta.y;

        toolTipObject.SetActive(false);
        dragObject.SetActive(false);
    }

    public override void SetSlot(ItemData itemData, int amount = 0)
    {
        this.icon.sprite = itemData.Icon;
        dragIcon.sprite = itemData.Icon;

        EquipmentToolTip(itemData.ID);

        sb.Append(itemData.ToolTip);
        itemName.text = itemData.ItemName;
        toolTipText.text = sb.ToString();

        this.icon.gameObject.SetActive(true);
    }

    void EquipmentToolTip(int id)
    {
        EquipableItemData eid = ItemMgr.Instance.GetItem(id) as EquipableItemData;
        sb.Clear();
        sb.Append("\n<color=blue>");
        if (eid.Status.attack != 0)
            sb.Append("공격력 : " + eid.Status.attack.ToString() + "\n");
        if (eid.Status.defence != 0)
            sb.Append("방어력 : " + eid.Status.defence.ToString() + "\n");
        if (eid.Status.recoveryMP != 0)
            sb.Append("MP 회복 속도 : " + eid.Status.recoveryMP.ToString() + "\n");
        if (eid.Status.recoverySP != 0)
            sb.Append("SP 회복 속도 : " + eid.Status.recoverySP.ToString() + "\n");
        if (eid.Status.guardGauge != 0)
            sb.Append("가드 게이지 : " + eid.Status.guardGauge.ToString() + "\n");
        sb.Append("</color>\n");
    }

    public override void ClearSlot()
    {
        icon.sprite = null;
        toolTipText.text = null;
        itemName.text = null;

        icon.gameObject.SetActive(false);
        dragObject.gameObject.SetActive(false);
        toolTipObject.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(HasItem && eventData.button == PointerEventData.InputButton.Right)
            InterfaceMgr.Instance.UnequipItem(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasItem)
        {
            toolTipObject.SetActive(true);
            toolTipObject.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInfo UI").transform);
            toolTipText.ForceMeshUpdate(true);
            float textHeight = (toolTipText.textInfo.lineCount - 1) * toolTipText.fontSize;
            RectTransform rectTr = toolTipObject.GetComponent<RectTransform>();
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, toolTipBgHeight + textHeight + 25);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipObject.gameObject.SetActive(false);
    }

    private bool IsOverUI()
=> EventSystem.current.IsPointerOverGameObject();
}
