using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SkillData;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] SkillData[] skill_datas;

    List<Weapon> player_weapons;
    List<Passive> player_passives;
    Dictionary<int, bool> check;

    private void Awake()
    {
        Array.Sort(skill_datas, (a, b) => (a.ID < b.ID ? -1 : 1));
        player_weapons = new();
        player_passives = new();
        check = new();
    }

    public void SetPlayerStartWeapon(SkillID weapon)
    {
        InterfaceManager.Instance.SetOwnSkill(skill_datas[(int)weapon]);
        WeaponInit((int)weapon);
        PassiveInit((int)SkillID.Get_Range_Up);
    }

    public void WeaponDamageUp(PassiveData.ApplicationType apply, float value)
    {
        SkillData.SkillType type = 0;
        switch (apply)
        {
            case PassiveData.ApplicationType.Sword:
                type = SkillData.SkillType.Sword;
                break;
            case PassiveData.ApplicationType.Magic:
                type = SkillData.SkillType.Magic;
                break;
        }

        foreach (Weapon weapon in player_weapons)
        {
            if (weapon.Type == type)
                weapon.PassiveApply(Weapon.Up.Damage, value);
        }
    }

    public void WeaponSpeedUp(PassiveData.ApplicationType apply, float value)
    {
        SkillData.SkillType type = 0;
        switch (apply)
        {
            case PassiveData.ApplicationType.Sword:
                type = SkillData.SkillType.Sword;
                break;
            case PassiveData.ApplicationType.Magic:
                type = SkillData.SkillType.Magic;
                break;
        }

        foreach (Weapon weapon in player_weapons)
        {
            if (weapon.Type == type)
                weapon.PassiveApply(Weapon.Up.Speed, value);
        }
    }

    public void WeaponScaleUp(PassiveData.ApplicationType apply, float value)
    {
        SkillData.SkillType type = 0;
        switch (apply)
        {
            case PassiveData.ApplicationType.Sword:
                type = SkillData.SkillType.Sword;
                break;
            case PassiveData.ApplicationType.Magic:
                type = SkillData.SkillType.Magic;
                break;
        }

        foreach (Weapon weapon in player_weapons)
        {
            if (weapon.Type == type)
                weapon.PassiveApply(Weapon.Up.Scale, value);
        }
    }

    public void GetRandomSkill()
    {
        int count = Mathf.Min(skill_datas.Length - 1, 3);   // 랜덤 스킬 뽑을 수 있는 횟수
        for (int i = 0; i < 3; i++) // 랜덤 스킬 3개 뽑기
        {
            if (count == 0) // 더이상 선택할게 없어 포션 선택
            {
                InterfaceManager.Instance.LevelUp((PassiveData)skill_datas[skill_datas.Length - 1], 0);
                continue;
            }

            int random_index = UnityEngine.Random.Range(0, skill_datas.Length - 1); // 랜덤 스킬 뽑기
            if (check.ContainsKey(random_index))    // 앞에 이미 뽑은 스킬인지 체크
            {
                i--;
                continue;
            }
            else
                check[random_index] = true;

            int index = player_passives.FindIndex(x => (int)skill_datas[random_index].ID == x.ID);  // 플레이어가 소유한 Passive 중에 있는지 확인
            if (index != -1)    // 만약 있다면
            {
                if (player_passives[index].Level == 5)  // 레벨5(최대 레벨)이면 패스
                {
                    i--;
                    continue;
                }
                InterfaceManager.Instance.LevelUp((PassiveData)skill_datas[random_index], player_passives[index].Level);
                count--;
                continue;
            }

            index = player_weapons.FindIndex(x => (int)skill_datas[random_index].ID == x.ID);   // 플레이어가 소유한 Weapon 중에 있는지 확인
            if (index != -1)    // 만약 있다면
            {
                if (player_weapons[index].Level == 5)   // 레벨5(최대 레벨)이면 패스
                {
                    i--;
                    continue;
                }
                InterfaceManager.Instance.LevelUp((WeaponData)skill_datas[random_index], player_weapons[index].Level);
                count--;
                continue;
            }

            if (skill_datas[random_index].Skill != SkillType.Passive)   // 랜덤 스킬이 Passive면
            {
                WeaponData skill = (WeaponData)skill_datas[random_index];
                InterfaceManager.Instance.LevelUp(skill, 0);
                count--;
            }
            else    // 랜덤 스킬이 Weapon이면
            {
                PassiveData skill = (PassiveData)skill_datas[random_index];
                InterfaceManager.Instance.LevelUp(skill, 0);
                count--;
            }
        }
        check.Clear();
    }

    public void SelectSkill(int id, bool isNew)
    {
        int index = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        if (isNew)  // 새롭게 얻은 스킬인 경우
        {
            if (skill_datas[index].Skill == SkillType.Portion)  // 물약을 선택했을 경우
            {
                PassiveData data = (PassiveData)skill_datas[skill_datas.Length - 1];
                GameManager.Instance.HPRecovery(data.LevelPerValue(0));
                return;
            }

            InterfaceManager.Instance.SetOwnSkill(skill_datas[index]);
            if (skill_datas[index].Skill == SkillType.Passive)
                PassiveInit(id);
            else
                WeaponInit(id);

        }
        else    // 기존에 있는 스킬을 골라 레벨업을 해야하는 경우
        {
            if (id >= (int)SkillID.Start && id <= (int)SkillID.WeaponEnd)
            {
                WeaponLevelUp(id);
                InterfaceManager.Instance.UpdateOwnSkillLevel(id);
            }
            else
            {
                PassiveLevelUp(id);
                InterfaceManager.Instance.UpdateOwnSkillLevel(id, true);
            }
        }
    }

    void WeaponInit(int id) // 새로운 Weapon Skill을 얻었을 때 실행하는 함수
    {
        int index = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        player_weapons.Add(skill_datas[index].SetWeapon());
        if (player_weapons.Count >= 5)  // Weapon Skill을 5개 얻었을 때
        {
            int index2 = 0;
            while (true)    // Skill_datas에서 소유한 Weapon Skill 5개를 제외한 모든 Weapon Skill을 배열에서 삭제함
            {
                if (skill_datas[index2].Skill == SkillType.Passive || skill_datas[index2].Skill == SkillType.Portion)
                {
                    index2++;
                    return;
                }

                if (player_weapons.FindIndex(x => x.ID == (int)skill_datas[index2].ID) == -1)
                {
                    skill_datas = skill_datas.Where(x => x.ID != skill_datas[index2].ID).ToArray();
                    index2--;
                }
                index2++;
            }
        }
    }

    void WeaponLevelUp(int id)
    {
        int index = player_weapons.FindIndex(x => id == x.ID);
        int level = player_weapons[index].Level - 1;
        int index2 = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        WeaponData data = (WeaponData)skill_datas[index2];
        player_weapons[index].LevelUp(data.LevelPerDamage(level), data.LevelPerSpeed(level), data.LevelPerPer(level), data.LevelPerCount(level), data.LevelPerKnockback(level));
        if (player_weapons[index].Level >= 5)   // Skill의 레벨이 5(최대 레벨)라면 
            skill_datas = skill_datas.Where(x => (int)x.ID != id).ToArray();    // skill_datas에서 삭제함
    }

    void PassiveInit(int id)
    {
        int index = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        player_passives.Add(skill_datas[index].SetPassive());
        if (player_passives.Count >= 5)
        {
            int index2 = 0;
            while (true)
            {
                if (skill_datas[index2].Skill == SkillType.Sword || skill_datas[index2].Skill == SkillType.Magic)
                {
                    index2++;
                    continue;
                }

                if (skill_datas[index2].Skill == SkillType.Portion)
                    return;

                if (player_passives.FindIndex(x => x.ID == (int)skill_datas[index2].ID) == -1)
                {
                    skill_datas = skill_datas.Where(x => x.ID != skill_datas[index2].ID).ToArray();
                    index2--;
                }
                index2++;
            }
        }
    }

    void PassiveLevelUp(int id)
    {
        int index = player_passives.FindIndex(x => id == x.ID);
        int level = player_passives[index].Level;
        int index2 = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        PassiveData data = (PassiveData)skill_datas[index2];
        player_passives[index].LevelUp(data.LevelPerValue(level));
        if (player_passives[index].Level >= 5)
            skill_datas = skill_datas.Where(x => (int)x.ID != id).ToArray();
    }

    public string GetWeaponName(SkillID id) { return skill_datas[(int)id].SkillName; }
}
