using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Slot : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] protected Image icon;

    public int Index { get; set; }
    public bool IsLeftClick { get; set; }
    public bool HasItem { get { return icon.sprite != null; } }

    protected MakeSlot.SlotType slotType;
    protected Transform parentTr;
    protected StringBuilder sb;

    public virtual void Initialize(MakeSlot.SlotType type)
    {
        slotType = type;
        IsLeftClick = false;
        parentTr = transform.parent;
        sb = new StringBuilder();
        transform.localScale = Vector3.one;
        icon.gameObject.SetActive(false);
    }

    public virtual void SetSlot(ItemData itemData, int amount = 0) { }
    public virtual void SetSlot(SkillData skillData) { }
    public virtual void ClearSlot() { }
    public virtual void UpdateInfo(int id) { }
    public virtual void SkillToolTipUpdate(SkillData data) { }
    public virtual int GetItemID() { return 0; }
}
