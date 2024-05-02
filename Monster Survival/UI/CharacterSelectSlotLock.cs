using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CharacterSelectSlotLock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI character_name_text;
    [SerializeField] TextMeshProUGUI ability_text;
    [SerializeField] TextMeshProUGUI start_weapon_text;
    [SerializeField] TextMeshProUGUI unlock_text;
    [SerializeField] Animator anim;

    StringBuilder sb;

    private void Awake()
    {
        sb = new();
    }

    public void SetCharacterDataLock(CharacterData data)
    {
        unlock_text.text = data.Unlock;
        anim.runtimeAnimatorController = null;

        ability_text.text = data.Ability;
        sb.Append("기본 스킬로 \"");
        sb.Append(GameManager.Instance.GetWeaponName(data.StartWeapon));
        sb.Append("\" 을 가지고 시작합니다.");
        start_weapon_text.text = sb.ToString();
        sb.Clear();

        character_name_text.text = data.CharacterName;
        anim.runtimeAnimatorController = data.Anim;
        anim.SetTrigger("Lock");
    }
}
