using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SkillData;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] SkillData[] skill_datas;

    System.Random random;
    List<Weapon> player_weapons;
    List<Passive> player_passives;

    private void Awake()
    {
        Array.Sort(skill_datas, (a, b) => (a.ID < b.ID ? -1 : 1));
        player_weapons = new List<Weapon>(5);
        player_passives = new List<Passive>(5);
        random = new();
    }

    public void SetPlayerStartWeapon(SkillID weapon)
    {
        InterfaceManager.Instance.SetOwnSkill(skill_datas[(int)weapon]);
        WeaponInit((int)weapon);
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
        int count = Mathf.Min(skill_datas.Length - 1, 3);   // ���� ��ų ���� �� �ִ� Ƚ��
        Shuffle();

        for (int i = 0; i < 3; i++) // ���� ��ų 3�� �̱�
        {
            if (i == count) // ���̻� ������ ��ų�� ��� �������� ���ǳ���
            {
                InterfaceManager.Instance.LevelUp((PassiveData)skill_datas[skill_datas.Length - 1], 0);
                continue;
            }

            int index = player_passives.FindIndex(x => (int)skill_datas[i].ID == x.ID);  // �÷��̾ ������ Passive �߿� �ִ��� Ȯ��
            if (index != -1)    // ���� �ִٸ�
            {
                InterfaceManager.Instance.LevelUp((PassiveData)skill_datas[i], player_passives[index].Level);
                //count--;
                continue;
            }

            index = player_weapons.FindIndex(x => (int)skill_datas[i].ID == x.ID);   // �÷��̾ ������ Weapon �߿� �ִ��� Ȯ��
            if (index != -1)    // ���� �ִٸ�
            {
                InterfaceManager.Instance.LevelUp((WeaponData)skill_datas[i], player_weapons[index].Level);
                //count--;
                continue;
            }

            if (skill_datas[i].Skill != SkillType.Passive)   // ���� ��ų�� Weapon�̸�
            {
                WeaponData skill = (WeaponData)skill_datas[i];
                InterfaceManager.Instance.LevelUp(skill, 0);
            }
            else    // ���� ��ų�� Passive��
            {
                PassiveData skill = (PassiveData)skill_datas[i];
                InterfaceManager.Instance.LevelUp(skill, 0);
            }
        }
    }

    // Modern Fisher-Yates Shuffle
    void Shuffle()
    {
        for (int i = 0; i < skill_datas.Length - 2; i++)
        {
            int r = random.Next(0, skill_datas.Length - 1);

            SkillData temp = skill_datas[i];
            skill_datas[i] = skill_datas[r];
            skill_datas[r] = temp;
        }
    }

    public void SelectSkill(int id, bool isNew)
    {
        int index = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        if (isNew)  // ���Ӱ� ���� ��ų�� ���
        {
            if (skill_datas[index].Skill == SkillType.Portion)  // ������ �������� ���
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
        else    // ������ �ִ� ��ų�� ��� �������� �ؾ��ϴ� ���
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

    void WeaponInit(int id) // ���ο� Weapon Skill�� ����� �� �����ϴ� �Լ�
    {
        int index = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        player_weapons.Add(skill_datas[index].SetWeapon());

        if (player_weapons.Count >= 5)  // Weapon Skill�� 5�� ����� ��
        {
            for (int i = 0; i < skill_datas.Length; i++)    // Skill_datas���� ������ Weapon Skill 5���� ������ ��� Weapon Skill�� �迭���� ������
            {
                if (skill_datas[i].Skill == SkillType.Passive || skill_datas[i].Skill == SkillType.Portion)
                    continue;

                if (player_weapons.FindIndex(x => x.ID == (int)skill_datas[i].ID) == -1)
                {
                    skill_datas = skill_datas.Where(x => x.ID != skill_datas[i].ID).ToArray();
                    i--;
                }
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
        if (player_weapons[index].Level >= 5)   // Skill�� ������ 5(�ִ� ����)��� 
            skill_datas = skill_datas.Where(x => (int)x.ID != id).ToArray();    // skill_datas���� ������
    }

    void PassiveInit(int id)
    {
        int index = Array.FindIndex(skill_datas, x => (int)x.ID == id);
        player_passives.Add(skill_datas[index].SetPassive());
        if (player_passives.Count >= 5)
        {
            for(int i=0;i<skill_datas.Length;i++)
            {
                if (skill_datas[i].Skill == SkillType.Sword || skill_datas[i].Skill == SkillType.Portion || skill_datas[i].Skill == SkillType.Magic)
                    continue;

                if(player_passives.FindIndex(x => x.ID == (int)skill_datas[i].ID) == -1)
                {
                    skill_datas = skill_datas.Where(x => x.ID != skill_datas[i].ID).ToArray();
                    i--;
                }
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
