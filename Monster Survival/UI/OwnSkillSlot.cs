using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class OwnSkillSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI name_text;
    [SerializeField] TextMeshProUGUI type_text;
    [SerializeField] TextMeshProUGUI desc_text;

    SkillData data;
    int level;
    public bool Check => icon.gameObject.activeSelf;

    public int ID => (int)data.ID;

    public void Initialize()
    {
        icon.gameObject.SetActive(false);
    }

    public void SetSkill(SkillData data)
    {
        this.data = data;
        icon.sprite = data.Icon;
        UpdateLevel();
        switch (data.Skill)
        {
            case SkillData.SkillType.Sword:
                type_text.text = "Sword";
                break;
            case SkillData.SkillType.Magic:
                type_text.text = "Magic";
                break;
            case SkillData.SkillType.Passive:
                type_text.text = "Passive";
                break;
        }

        if (data.Skill == SkillData.SkillType.Passive)
            UpdateDesc();
        else
            desc_text.text = data.SkillDesc;
        icon.gameObject.SetActive(true);
    }

    public void UpdateLevel(bool isPassive = false)
    {
        level++;
        StringBuilder sb = new();
        sb.Append(data.SkillName);
        sb.Append(" (Lv.");
        sb.Append(level.ToString());
        sb.Append(")");
        name_text.text = sb.ToString();
        sb.Clear();
        if (isPassive)
            UpdateDesc();
    }

    void UpdateDesc()
    {
        PassiveData passive = (PassiveData)data;
        desc_text.text = string.Format(passive.SkillDesc, passive.LevelPerValue(level - 1));
    }
}
