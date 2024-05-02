using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speed_text;
    [SerializeField] TextMeshProUGUI hp_text;
    [SerializeField] TextMeshProUGUI mp_text;
    [SerializeField] TextMeshProUGUI skill_damage_text;
    [SerializeField] TextMeshProUGUI character_name_text;
    [SerializeField] Animator anim;
    [SerializeField] UpgradeUI upgradeUI;
    [SerializeField] TextMeshProUGUI ability_text;
    [SerializeField] TextMeshProUGUI start_weapon_text;


    StringBuilder sb;
    CharacterData data;

    private void Awake()
    {
        sb = new();
    }

    public void SetCharacterData(CharacterData data)
    {
        this.data = data;

        DisPlayInfo();

        ability_text.text = data.Ability;
        sb.Append("기본 스킬로 \"");
        sb.Append(GameManager.Instance.GetWeaponName(data.StartWeapon));
        sb.Append("\" 을 가지고 시작합니다.");
        start_weapon_text.text = sb.ToString();
        sb.Clear();
        character_name_text.text = data.CharacterName;
        anim.runtimeAnimatorController = data.Anim;
        anim.Rebind();
        anim.SetTrigger("Select");
    }

    public void DisPlayInfo()
    {
        sb.Append("이동속도 : ");
        sb.Append(((decimal)data.MoveSpeed - (decimal)data.Upgrade.GetMoveSpeed()).ToString());
        sb.Append("  +  <color=yellow>");
        sb.Append(data.Upgrade.GetMoveSpeed().ToString());
        sb.Append("</color>");
        speed_text.text = sb.ToString();
        sb.Clear();

        sb.Append("최대 체력 : ");
        sb.Append(((decimal)data.HP - (decimal)data.Upgrade.GetHP()).ToString());
        sb.Append("  +  <color=yellow>");
        sb.Append(data.Upgrade.GetHP().ToString());
        sb.Append("</color>");
        hp_text.text = sb.ToString();
        sb.Clear();

        sb.Append("마나 회복량 : ");
        sb.Append(((decimal)data.MPPlus - (decimal)data.Upgrade.GetMPPlus()).ToString());
        sb.Append("  +  <color=yellow>");
        sb.Append(data.Upgrade.GetMPPlus().ToString());
        sb.Append("</color>");
        mp_text.text = sb.ToString();
        sb.Clear();

        sb.Append("스킬 데미지 : ");
        sb.Append(((decimal)data.SkillDamage - (decimal)data.Upgrade.GetSkillDamage()).ToString());
        sb.Append("  +  <color=yellow>");
        sb.Append(data.Upgrade.GetSkillDamage().ToString());
        sb.Append("</color>");
        skill_damage_text.text = sb.ToString();
        sb.Clear();
    }

    public void ClickUpgrade()
    {
        upgradeUI.gameObject.SetActive(true);
        upgradeUI.SetUpgradeInfo(data);
    }

    public void ClickGameStart() { GameManager.Instance.ShowLoadingUI(data); }
}
