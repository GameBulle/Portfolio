using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUISlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    [Header("Drag")]
    [SerializeField] Image dragIcon;
    [SerializeField] GameObject dragObject;

    [Header("ToolTip")]
    [SerializeField] GameObject toolTipObject;
    [SerializeField] TextMeshProUGUI toolTipText;
    [SerializeField] TextMeshProUGUI skillName;

    bool startDrag;
    float toolTipBgHeight;

    public override void Initialize(MakeSlot.SlotType type)
    {
        base.Initialize(type);

        startDrag = false;

        RectTransform rectTr = toolTipObject.GetComponent<RectTransform>();
        toolTipBgHeight = rectTr.sizeDelta.y;

        toolTipObject.SetActive(false);
        dragObject.SetActive(false);
    }

    public override void SetSlot(SkillData skillData)
    {
        this.icon.sprite = skillData.Icon;
        dragIcon.sprite = skillData.Icon;

        SkillToolTipUpdate(skillData);

        this.icon.gameObject.SetActive(true);
    }

    public override void SkillToolTipUpdate(SkillData skillData)
    {
        sb.Clear();
        sb.Append(skillData.ToolTip);
        sb.Append("\n<color=blue>");
        switch(skillData.Id)
        {
            case 0:
                sb.Append("���� ���� ���� ��� ���Ϳ��� ��ų����(" + skillData.Level.ToString() + ") * ���ݷ�("
                    + skillData.Damage.ToString() + ") ��ŭ ������(" + (skillData.Level * skillData.Damage) + ")�� �ݴϴ�.\n");
                break;
            case 1:
                sb.Append("���� ���߽� ��ų����(" + skillData.Level.ToString() + ") * ���ݷ�("
                    + skillData.Damage.ToString() + ") ��ŭ ������(" + (skillData.Level * skillData.Damage) + ")�� �ְ� ���� ����� ���Ϳ��� ���� ������ ������ ������ �������� �ݴϴ�. (�ִ� 5��)\n");
                break;
            case 2:
                sb.Append("�⸦ ��� �������� �����ؼ� ū ������ ����Ű�� ���Ϳ��� ��ų����(" + skillData.Level.ToString() + " ) * ���ݷ�("
                    + skillData.Damage.ToString() + ") ��ŭ ������(" + (skillData.Level * skillData.Damage) + ")�� �ݴϴ�.");
                break;
            case 3:

                break;
        }
        sb.Append("</color>");
        toolTipText.text = sb.ToString();
        skillName.text = skillData.SkillName + "\n";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HasItem)
        {
            dragObject.SetActive(true);
            startDrag = true;
            dragObject.transform.SetParent(GameObject.FindGameObjectWithTag("Skill UI").transform);
            //GameObject.FindGameObjectWithTag("Skill UI").GetComponent<Canvas>().sortingOrder = 10;

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
            InterfaceMgr.Instance.InitDragInfo();
        else
            InterfaceMgr.Instance.DragSkill();

        startDrag = false;
        dragObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        InterfaceMgr.Instance.SetDragEndInfo(slotType, Index, HasItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipObject.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasItem)
        {
            toolTipObject.SetActive(true);
            toolTipObject.transform.SetParent(GameObject.FindGameObjectWithTag("Skill UI").transform);
            toolTipText.ForceMeshUpdate(true);
            float textHeight = (toolTipText.textInfo.lineCount - 1) * toolTipText.fontSize;
            RectTransform rectTr = toolTipObject.GetComponent<RectTransform>();
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, toolTipBgHeight + textHeight + 25);
        }
    }

    private bool IsOverUI()
    => EventSystem.current.IsPointerOverGameObject();
}
