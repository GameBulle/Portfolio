using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Slot, IPointerClickHandler,IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    [Header("Drag")]
    [SerializeField] Image dragIcon;
    [SerializeField] TextMeshProUGUI dragAmountText;
    [SerializeField] GameObject dragObject;

    [SerializeField] TextMeshProUGUI amountText;

    [Header("ToolTip")]
    [SerializeField] GameObject toolTipObject;
    [SerializeField] TextMeshProUGUI toolTipText;
    [SerializeField] TextMeshProUGUI itemName;

    bool startDrag;
    bool inputShift;
    float toolTipBgHeight;
    int itemID;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            inputShift = true;
        else
            inputShift = false;
    }

    public override void Initialize(MakeSlot.SlotType type)
    {
        base.Initialize(type);

        inputShift = false;
        startDrag = false;

        RectTransform rectTr = toolTipObject.GetComponent<RectTransform>();
        toolTipBgHeight = rectTr.sizeDelta.y;

        toolTipObject.SetActive(false);
        amountText.gameObject.SetActive(false);
        dragObject.SetActive(false);
    }

    public override void SetSlot(ItemData itemData, int amount)
    {
        sb.Clear();
        if (itemData.ID == -1)
            ClearSlot();

        this.icon.sprite = itemData.Icon;
        dragIcon.sprite = itemData.Icon;
        itemID = itemData.ID;

        if (itemData.Type == ItemData.ItemType.Equipable)
            EquipmentToolTip(itemData.ID);
        else if(itemData.Type == ItemData.ItemType.Countable)
        {
            amountText.gameObject.SetActive(true);
            dragAmountText.gameObject.SetActive(true);

            amountText.text = amount.ToString();
            dragAmountText.text = amount.ToString();
        }
        sb.Append(itemData.ToolTip);
        itemName.text = itemData.ItemName;
        toolTipText.text = sb.ToString();

        this.icon.gameObject.SetActive(true);
    }

    void EquipmentToolTip(int id)
    {
        amountText.gameObject.SetActive(false);
        dragAmountText.gameObject.SetActive(false);
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

    public override void ClearSlot()
    {
        icon.sprite = null;
        amountText.text = null;
        toolTipText.text = null;
        itemName.text = null;
        itemID = -1;

        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
        dragObject.gameObject.SetActive(false);
        toolTipObject.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(HasItem)
        {
            dragObject.SetActive(true);
            startDrag = true;
            dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory UI").transform);
            GameObject.FindGameObjectWithTag("Inventory UI").GetComponent<Canvas>().sortingOrder = 10;

            InterfaceMgr.Instance.SetDragStartIndex(slotType, Index);
            toolTipObject.gameObject.SetActive(false);
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

        if (!IsOverUI())
        {
            if (amountText.gameObject.activeSelf == true)
            {
                if (inputShift && startDrag)
                    InterfaceMgr.Instance.InputUIActive(dragObject.transform.position, int.Parse(amountText.text), Index, slotType);
                else
                    InterfaceMgr.Instance.ClearSlot(slotType, Index);
            }
            else
                InterfaceMgr.Instance.ClearSlot(slotType, Index);
        }
        else
            InterfaceMgr.Instance.DragItem();

        startDrag = false;
        inputShift = false;
        dragObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Inventory UI").GetComponent<Canvas>().sortingOrder = 2;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InterfaceMgr.Instance.SetDragEndInfo(slotType, Index, HasItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipObject.gameObject.SetActive(false);
        sb.Clear();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasItem)
        {
            toolTipObject.SetActive(true);
            toolTipObject.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory UI").transform);
            toolTipText.ForceMeshUpdate(true);
            float textHeight = (toolTipText.textInfo.lineCount - 1) * toolTipText.fontSize;
            RectTransform rectTr = toolTipObject.GetComponent<RectTransform>();
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, toolTipBgHeight + textHeight + 25);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(HasItem && eventData.button == PointerEventData.InputButton.Right)
        {
            if(GameMgr.Instance.isShop)
            {
                SoundMgr.Instance.PlaySFXAudio("Sell");
                InterfaceMgr.Instance.AddSellItemToArray(itemID,Index);
                return;
            }
            else
            {
                InterfaceMgr.Instance.GetUseItemSlotIndex(Index);
                return;
            }
        }
    }

    private bool IsOverUI()
    => EventSystem.current.IsPointerOverGameObject();
}
