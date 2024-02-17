using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Icon")]
    [SerializeField] Image icon;
    [SerializeField] Image dragItemIcon;

    [Header("Item Count")]
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI dragAmountText;

    [Header("Quick Number")]
    [SerializeField] TextMeshProUGUI quickNumber;

    [Header("Tultip Text")]
    [SerializeField] TextMeshProUGUI tooltip;
    [SerializeField] TextMeshProUGUI itemName;

    [Header("Highlight Image")]
    [SerializeField] Image highlightImg;

    [Header("Drag Object")]
    [SerializeField] GameObject dragObject;

    [Header("Tultip Object")]
    [SerializeField] GameObject tooltipObject;

    public int Index { get; set; }

    public bool HasItem { get { return icon.sprite != null; } }

    public bool IsAccessible { get { return (isAccessibleSlot && IsAccessibleItem); } }

    MakeSlot.SlotType slotType;
    private bool isAccessibleSlot;
    private bool IsAccessibleItem;
    public bool IsLeftClick;
    bool startDrag;
    bool inputShift;
    Transform parentTr;
    float tooltipBgHeight;

    private void Update()
    {
        if (slotType == MakeSlot.SlotType.Inventory && Input.GetKey(KeyCode.LeftShift))
            inputShift = true;
        else
            inputShift = false;
    }

    public void Initialize(MakeSlot.SlotType type)
    {
        IsLeftClick = false;
        inputShift = false;
        slotType = type;
        if(slotType == MakeSlot.SlotType.Quick)
        {
            quickNumber.gameObject.SetActive(true);
            quickNumber.text = (Index + 1).ToString();
        }
        else if(slotType == MakeSlot.SlotType.SkillQuick)
        {
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
        else
            quickNumber.gameObject.SetActive(false);
        parentTr = transform.parent;
        RectTransform rectTr = tooltipObject.GetComponent<RectTransform>();
        tooltipBgHeight = rectTr.sizeDelta.y;
        startDrag = false;

        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
        highlightImg.gameObject.SetActive(false);
        tooltipObject.gameObject.SetActive(false);
        dragObject.SetActive(false);
    }

    public void SetSlot(ItemData itemData, int amount)
    {
        if (amount == 0)
        {
            ClearSlot();
            return;
        }

        this.icon.sprite = itemData.Icon;
        dragItemIcon.sprite = itemData.Icon;
        tooltip.text = null;

        EquipableItemData eid;
        if(eid = itemData as EquipableItemData)
        {
            amountText.gameObject.SetActive(false);
            dragAmountText.gameObject.SetActive(false);

            tooltip.text += "\n<color=blue>";
            if (eid.Status.attack != 0)
                tooltip.text += "공격력 : " + eid.Status.attack.ToString() + "\n";
            if (eid.Status.defence != 0)
                tooltip.text += "방어력 : " + eid.Status.defence.ToString() + "\n";
            if( eid.Status.recoveryMP != 0)
                tooltip.text += "MP 회복 속도 : " + eid.Status.recoveryMP.ToString() + "\n";
            if(eid.Status.recoverySP != 0)
                tooltip.text += "SP 회복 속도 : " + eid.Status.recoverySP.ToString() + "\n";
            if (eid.Status.guardGauge != 0)
                tooltip.text += "가드 게이지 : " + eid.Status.guardGauge.ToString() + "\n";
            tooltip.text += "</color>\n";
        }
        else
        {
            amountText.gameObject.SetActive(true);
            dragAmountText.gameObject.SetActive(true);

            amountText.text = amount.ToString();
            dragAmountText.text = amount.ToString();
        }

        tooltip.text += itemData.ToolTip;
        itemName.text = itemData.ItemName;

        this.icon.gameObject.SetActive(true);
    }

    public void SetSlot(SkillData skillData)
    {
        this.icon.sprite = skillData.Icon;
        dragItemIcon.sprite = skillData.Icon;

        SkillTooltipUpdate(skillData);

        amountText.gameObject.SetActive(false);
        dragAmountText.gameObject.SetActive(false);
        highlightImg.gameObject.SetActive(false);
        this.icon.gameObject.SetActive(true);
    }

    public void SkillTooltipUpdate(SkillData skillData)
    {
        tooltip.text = null;
        tooltip.text = skillData.ToolTip;
        tooltip.text += "\n<color=blue>";
        switch (skillData.Id)
        {
            case 0:
                tooltip.text += "폭발 범위 안의 모든 몬스터에게 스킬레벨(" + skillData.Level.ToString() + ") * 공격력("
                    + skillData.Damage.ToString() + ") 만큼 데미지(" + (skillData.Level * skillData.Damage) + ")를 줍니다.\n";
                break;
            case 1:
                tooltip.text += "몬스터 적중시 스킬레벨(" + skillData.Level.ToString() + ") * 공격력("
                    + skillData.Damage.ToString() + ") 만큼 데미지(" + (skillData.Level * skillData.Damage) + ")를 주고 제일 가까운 몬스터에게 연쇄 반응을 일으켜 동일한 데미지를 줍니다. (최대 5번)\n";
                break;
        }
        tooltip.text += "</color>";
        itemName.text = skillData.SkillName + "\n";
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        amountText.text = null;
        IsLeftClick = false;
        tooltip.text = null;
        itemName.text = null;

        highlightImg.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
        dragObject.gameObject.SetActive(false);
        tooltipObject.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(HasItem)
        {
            if ((slotType == MakeSlot.SlotType.Inventory || slotType == MakeSlot.SlotType.Quick) && eventData.button == PointerEventData.InputButton.Right)
            {
                InterfaceMgr.Instance.GetUseItemSlotIndex(Index);
            }
            else if (slotType == MakeSlot.SlotType.DropItemBox)
            {
                IsLeftClick = !IsLeftClick;
                highlightImg.gameObject.SetActive(IsLeftClick);
            }
            else if (slotType == MakeSlot.SlotType.Equipment && eventData.button == PointerEventData.InputButton.Right)
            {
                InterfaceMgr.Instance.UnequipItem(Index);
            }
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(HasItem && (slotType == MakeSlot.SlotType.Inventory || slotType == MakeSlot.SlotType.Quick || slotType == MakeSlot.SlotType.Equipment || slotType == MakeSlot.SlotType.SkillUI || slotType == MakeSlot.SlotType.SkillQuick))
        {
            dragObject.SetActive(true);
            startDrag = true;

            switch(slotType)
            {
                case MakeSlot.SlotType.Inventory:
                    dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory UI").transform);
                    GameObject.FindGameObjectWithTag("Inventory UI").GetComponent<Canvas>().sortingOrder = 2;
                    //GameObject.FindGameObjectWithTag("PlayerInfo UI").GetComponent<Canvas>().sortingOrder = 1;
                    GameObject.FindGameObjectWithTag("QuickSlot UI").GetComponent<Canvas>().sortingOrder = 0;
                    break;
                case MakeSlot.SlotType.Quick:
                    dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("QuickSlot UI").transform);
                    GameObject.FindGameObjectWithTag("Inventory UI").GetComponent<Canvas>().sortingOrder = 1;
                    //GameObject.FindGameObjectWithTag("PlayerInfo UI").GetComponent<Canvas>().sortingOrder = 0;
                    GameObject.FindGameObjectWithTag("QuickSlot UI").GetComponent<Canvas>().sortingOrder = 2;
                    break;
                case MakeSlot.SlotType.Equipment:
                    dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInfo UI").transform);
                    GameObject.FindGameObjectWithTag("Inventory UI").GetComponent<Canvas>().sortingOrder = 1;
                    //GameObject.FindGameObjectWithTag("PlayerInfo UI").GetComponent<Canvas>().sortingOrder = 2;
                    GameObject.FindGameObjectWithTag("QuickSlot UI").GetComponent<Canvas>().sortingOrder = 0;
                    break;
                case MakeSlot.SlotType.SkillUI:
                    dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("Skill UI").transform);
                    break;
            }
            InterfaceMgr.Instance.SetDragStartIndex(slotType, Index);
            tooltipObject.gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!startDrag)
            return;
        dragObject.transform.position = eventData.position;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!startDrag)
            return;

        dragObject.transform.SetParent(parentTr);
        dragObject.transform.localPosition = Vector3.zero;

        if(!IsOverUI() && slotType != MakeSlot.SlotType.SkillUI)
        {
            if (amountText.gameObject.activeSelf == true)
            {
                if (inputShift && startDrag)
                    InterfaceMgr.Instance.InputUIActive(dragObject.transform.position, int.Parse(amountText.text), Index, slotType);
                else
                    ClearSlot();
            }
            else
                ClearSlot();
        }
        else if (slotType == MakeSlot.SlotType.SkillUI || slotType == MakeSlot.SlotType.SkillQuick)
            InterfaceMgr.Instance.DragSkill();
        else
            InterfaceMgr.Instance.DragItem();

        startDrag = false;
        inputShift = false;
        dragObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        InterfaceMgr.Instance.SetDragEndInfo(slotType, Index, HasItem);
    }

    private bool IsOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(HasItem && !startDrag)
        {
            tooltipObject.gameObject.SetActive(true);
            switch (slotType)
            {
                case MakeSlot.SlotType.Inventory:
                    tooltipObject.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory UI").transform);
                    break;
                case MakeSlot.SlotType.DropItemBox:
                    tooltipObject.transform.SetParent(GameObject.FindGameObjectWithTag("DropItemBox UI").transform);
                    break;
                case MakeSlot.SlotType.Equipment:
                    tooltipObject.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInfo UI").transform);
                    break;
                case MakeSlot.SlotType.SkillUI:
                    tooltipObject.transform.SetParent(GameObject.FindGameObjectWithTag("Skill UI").transform);
                    break; ;
                default:
                    tooltipObject.gameObject.SetActive(false);
                    break;
            }
            
            tooltip.ForceMeshUpdate(true);
            float textHeight = (tooltip.textInfo.lineCount - 1) * tooltip.fontSize;
            RectTransform rectTr = tooltipObject.GetComponent<RectTransform>();
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, tooltipBgHeight + textHeight + 25);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipObject.gameObject.SetActive(false);
    }
}
