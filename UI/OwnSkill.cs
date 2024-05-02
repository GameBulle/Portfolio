using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnSkill : MonoBehaviour
{
    [SerializeField] OwnSkillSlot[] weapons;
    [SerializeField] OwnSkillSlot[] passives;

    public void Initialize()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Initialize();
            passives[i].Initialize();
        }
        gameObject.SetActive(false);
    }

    public void SetOwnSkill(SkillData data)
    {
        switch (data.Skill)
        {
            case SkillData.SkillType.Sword:
            case SkillData.SkillType.Magic:
                for (int i = 0; i < weapons.Length; i++)
                {
                    if (!weapons[i].Check)
                    {
                        weapons[i].SetSkill(data);
                        return;
                    }
                }
                break;
            case SkillData.SkillType.Passive:
                for (int i = 0; i < passives.Length; i++)
                {
                    if (!passives[i].Check)
                    {
                        passives[i].SetSkill(data);
                        return;
                    }
                }
                break;
        }
    }

    public void UpdateOwnSkillLevel(int id, bool isPassive = false)
    {
        int index;
        if(isPassive)
        {
            index = Array.FindIndex(passives, x => x.ID == id);
            passives[index].UpdateLevel(true);
        }
        else
        {
            index = Array.FindIndex(weapons, x => x.ID == id);
            weapons[index].UpdateLevel();
        }
    }

    public void Show() { gameObject.SetActive(true); }
    public void Hide() { gameObject.SetActive(false); }
}
