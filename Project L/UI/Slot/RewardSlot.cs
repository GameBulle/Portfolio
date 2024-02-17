using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardSlot : Slot, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] TextMeshProUGUI amountText;

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
        amountText.gameObject.SetActive(false);
    }

    public override void SetSlot(ItemData itemData, int amount = 0)
    {
        sb.Clear();
        if (itemData.ID == -1)
            return;

        this.icon.sprite = itemData.Icon;

        if (itemData.Type == ItemData.ItemType.Equipable)
            EquipmentToolTip(itemData.ID);
        else if (itemData.Type == ItemData.ItemType.Countable)
        {
            amountText.gameObject.SetActive(true);
            amountText.text = amount.ToString();
        }
        sb.Append(itemData.ToolTip);
        itemName.text = itemData.ItemName;
        toolTipText.text = sb.ToString();

        this.icon.gameObject.SetActive(true);
    }

    void EquipmentToolTip(int id)
    {
        amountText.gameObject.SetActive(false);
        EquipableItemData eid = ItemMgr.Instance.GetItem(id) as EquipableItemData;

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


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasItem)
        {
            toolTipObject.SetActive(true);
            toolTipObject.transform.SetParent(GameObject.FindGameObjectWithTag("QuestWindow UI").transform);

            toolTipText.ForceMeshUpdate(true);
            float textHeight = (toolTipText.textInfo.lineCount - 1) * toolTipText.fontSize;
            RectTransform rectTr = toolTipObject.GetComponent<RectTransform>();
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, toolTipBgHeight + textHeight + 25);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipObject.gameObject.SetActive(false);
        sb.Clear();
    }
}
