using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI name_text;
    [SerializeField] TextMeshProUGUI type_text;
    [SerializeField] TextMeshProUGUI desc_text;

    int id;
    int level;  

    public void SetSkill(WeaponData data, int level)
    {
        gameObject.SetActive(true);
        StringBuilder sb = new();
        icon.sprite = data.Icon;
        id = (int)data.ID;
        this.level = level;

        // Item Name Text
        sb.Append(data.SkillName);
        if (level == 0)
            sb.Append(" (New!)");
        else
        {
            sb.Append(" (Lv.");
            sb.Append(level.ToString());
            sb.Append(")");
        }
        name_text.text = sb.ToString();
        sb.Clear();

        // Item Type Text
        switch (data.Skill)
        {
            case WeaponData.SkillType.Sword:
                type_text.text = "Sword";
                break;
            case WeaponData.SkillType.Magic:
                type_text.text = "Magic";
                break;
        }

        if (level == 0)
            desc_text.text = data.SkillDesc;
        else
        {
            sb.Append("Lv.");
            sb.Append(level.ToString());
            sb.Append(" -> Lv.");
            sb.AppendLine((level + 1).ToString());
            if (data.LevelPerDamage(level - 1) != 0)
                sb.AppendLine(string.Format("데미지 {0:P0} 증가", data.LevelPerDamage(level - 1)));

            if (data.LevelPerSpeed(level - 1) != 0)
                sb.AppendLine(string.Format("연사(회전) 속도 {0:P0} 증가", data.LevelPerSpeed(level - 1)));

            if (data.LevelPerPer(level - 1) != 0)
                sb.AppendLine(string.Format("관통력 {0:D} 증가", data.LevelPerPer(level - 1)));

            if (data.LevelPerCount(level - 1) != 0)
                sb.AppendLine(string.Format("개수 {0:D} 증가", data.LevelPerCount(level - 1)));

            if (data.LevelPerKnockback(level - 1) != 0)
                sb.AppendLine(string.Format("넉백 {0:F} 증가", data.LevelPerKnockback(level - 1)));

            desc_text.text = sb.ToString();
        }
        sb.Clear();
    }

    public void SetSkill(PassiveData data, int level)
    {
        gameObject.SetActive(true);
        StringBuilder sb = new();
        icon.sprite = data.Icon;
        id = (int)data.ID;
        this.level = level;


        // Item Name Text
        sb.Append(data.SkillName);

        if (data.ID != SkillData.SkillID.Portion)
        {
            if (level == 0)
                sb.Append(" (New!)");
            else
            {
                sb.Append(" (Lv.");
                sb.Append(level.ToString());
                sb.Append(")");
            }
        }
        name_text.text = sb.ToString();
        sb.Clear();

        // Item Type Text
        if (data.ID == SkillData.SkillID.Portion)
            type_text.text = "Portion";
        else
        {
            type_text.text = "Passive";
            sb.Append("Lv.");
            sb.Append(level.ToString());
            sb.Append(" -> Lv.");
            sb.AppendLine((level + 1).ToString());
        }
        sb.Append(string.Format(data.SkillDesc, data.LevelPerValue(level)));
        desc_text.text = sb.ToString();
        sb.Clear();
    }

    public void OnClick()
    {
        if (level == 0)
            GameManager.Instance.SelectSkill(id, true);
        else
            GameManager.Instance.SelectSkill(id);
    }
}
