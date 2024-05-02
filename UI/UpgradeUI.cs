using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI move_speed_text;
    [SerializeField] TextMeshProUGUI move_speed_value_text;
    [SerializeField] TextMeshProUGUI move_speed_gold_text;
    [SerializeField] TextMeshProUGUI hp_text;
    [SerializeField] TextMeshProUGUI hp_value_text;
    [SerializeField] TextMeshProUGUI hp_gold_text;
    [SerializeField] TextMeshProUGUI mp_plus_text;
    [SerializeField] TextMeshProUGUI mp_plus_value_text;
    [SerializeField] TextMeshProUGUI mp_plus_gold_text;
    [SerializeField] TextMeshProUGUI skill_damage_text;
    [SerializeField] TextMeshProUGUI skill_damage_value_text;
    [SerializeField] TextMeshProUGUI skill_damage_gold_text;

    CharacterData data;
    StringBuilder sb;

    private void Awake()
    {
        sb = new();
    }

    public void SetUpgradeInfo(CharacterData data)
    {
        this.data = data;
        DisPlayMoveSpeed();
        DisPlayHP();
        DisPlayMPPlus();
        DisPlaySkillDamage();
    }

    void DisPlayMoveSpeed()
    {
        int level = data.Upgrade.GetMoveSpeedLevel();

        sb.Append("이동 속도(");
        if(level >= 5)
        {
            sb.Append("<color=red>Lv.Max</color>)");
            move_speed_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=red>");
        }
        else
        {
            sb.Append("<color=yellow>Lv.");
            sb.Append(level.ToString());
            sb.Append("</color>)");
            move_speed_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=yellow>");
        }
        sb.Append(data.Upgrade.GetMoveSpeed().ToString());
        sb.Append("</color>");
        move_speed_value_text.text = sb.ToString();
        sb.Clear();

        if(level >= 5)
            sb.Append("<color=red>Max</color>");
        else
        {
            sb.Append("<color=green>Upgrade(");
            sb.Append(data.Upgrade.GetMoveSpeedGold().ToString());
            sb.Append(")</color>");
        }
        move_speed_gold_text.text = sb.ToString();
        sb.Clear();
    }

    void DisPlayHP()
    {
        int level = data.Upgrade.GetHPLevel();

        sb.Append("최대 체력(");
        if (level >= 5)
        {
            sb.Append("<color=red>Lv.Max</color>)");
            hp_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=red>");
        }
        else
        {
            sb.Append("<color=yellow>Lv.");
            sb.Append(level.ToString());
            sb.Append("</color>)");
            hp_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=yellow>");
        }
        sb.Append(data.Upgrade.GetHP().ToString());
        sb.Append("</color>");
        hp_value_text.text = sb.ToString();
        sb.Clear();

        if (level >= 5)
            sb.Append("<color=red>Max</color>");
        else
        {
            sb.Append("<color=green>Upgrade(");
            sb.Append(data.Upgrade.GetHPGold().ToString());
            sb.Append(")</color>");
        }
        hp_gold_text.text = sb.ToString();
        sb.Clear();
    }

    void DisPlayMPPlus()
    {
        int level = data.Upgrade.GetMPPlusLevel();

        sb.Append("마나 회복량(");
        if (level >= 5)
        {
            sb.Append("<color=red>Lv.Max</color>)");
            mp_plus_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=red>");
        }
        else
        {
            sb.Append("<color=yellow>Lv.");
            sb.Append(level.ToString());
            sb.Append("</color>)");
            mp_plus_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=yellow>");
        }
        sb.Append(data.Upgrade.GetMPPlus().ToString());
        sb.Append("</color>");
        mp_plus_value_text.text = sb.ToString();
        sb.Clear();

        if (level >= 5)
            sb.Append("<color=red>Max</color>");
        else
        {
            sb.Append("<color=green>Upgrade(");
            sb.Append(data.Upgrade.GetMPPlusGold().ToString());
            sb.Append(")</color>");
        }
        mp_plus_gold_text.text = sb.ToString();
        sb.Clear();
    }

    void DisPlaySkillDamage()
    {
        int level = data.Upgrade.GetSkillDamageLevel();

        sb.Append("스킬 데미지(");
        if (level >= 5)
        {
            sb.Append("<color=red>Lv.Max</color>)");
            skill_damage_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=red>");
        }
        else
        {
            sb.Append("<color=yellow>Lv.");
            sb.Append(level.ToString());
            sb.Append("</color>)");
            skill_damage_text.text = sb.ToString();
            sb.Clear();
            sb.Append("<color=yellow>");
        }
        sb.Append(data.Upgrade.GetSkillDamage().ToString());
        sb.Append("</color>");
        skill_damage_value_text.text = sb.ToString();
        sb.Clear();

        if (level >= 5)
            sb.Append("<color=red>Max</color>");
        else
        {
            sb.Append("<color=green>Upgrade(");
            sb.Append(data.Upgrade.GetSkillDamageGold().ToString());
            sb.Append(")</color>");
        }
        skill_damage_gold_text.text = sb.ToString();
        sb.Clear();
    }

    public void ClickUpgradeMoveSpeed()
    {
        int gold = GameManager.Instance.Gold;
        if(data.Upgrade.MoveSpeedUpgrade(ref gold))
        {
            GameManager.Instance.Gold = gold;
            DisPlayMoveSpeed();
            GameManager.Instance.SaveGameData();
        }
    }

    public void ClickUpgradeHP()
    {
        int gold = GameManager.Instance.Gold;
        if(data.Upgrade.HPUpgrade(ref gold))
        {
            GameManager.Instance.Gold = gold;
            DisPlayHP();
            GameManager.Instance.SaveGameData();
        }
    }

    public void ClickUpgradeMPPlus()
    {
        int gold = GameManager.Instance.Gold;
        if(data.Upgrade.MPPlusUpgrade(ref gold))
        {
            GameManager.Instance.Gold = gold;
            DisPlayMPPlus();
            GameManager.Instance.SaveGameData();
        }
    }

    public void ClickUpgradeSkillDamage()
    {
        int gold = GameManager.Instance.Gold;
        if(data.Upgrade.SkillDamageUpgrade(ref gold))
        {
            GameManager.Instance.Gold = gold;
            DisPlaySkillDamage();
            GameManager.Instance.SaveGameData();
        }
    }
}
