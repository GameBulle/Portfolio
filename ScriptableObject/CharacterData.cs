using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Object/CharacterData")]
public class CharacterData : ScriptableObject
{
    public enum CharacterType { Warrior, Wizard, Samurai, Shaman }

    [SerializeField] CharacterType id;
    [SerializeField] RuntimeAnimatorController anim;
    [SerializeField] SkillData.SkillID start_weapon;
    [SerializeField] string character_name;
    [SerializeField] float move_speed;
    [SerializeField] float hp;
    [SerializeField] float mp_plus;
    [SerializeField] float skill_use_mp;
    [SerializeField] float skill_damage;
    [SerializeField] Upgrade upgrade;
    [TextArea]
    [SerializeField] string ability;
    [TextArea]
    [SerializeField] string unlock;
    [SerializeField] bool isLock;
    [SerializeField] AudioClip clip;

    public CharacterType ID => id;
    public RuntimeAnimatorController Anim => anim;
    public SkillData.SkillID StartWeapon => start_weapon;
    public string CharacterName => character_name;
    public float MoveSpeed => move_speed + upgrade.GetMoveSpeed();
    public float HP => hp + upgrade.GetHP();
    public float MPPlus => mp_plus + upgrade.GetMPPlus();
    public float SkillUseMp => skill_use_mp;
    public float SkillDamage => skill_damage + upgrade.GetSkillDamage();
    public Upgrade Upgrade => upgrade;
    public string Ability => ability;
    public string Unlock => unlock;
    public AudioClip Clip => clip;
    public bool IsLock { get { return isLock; } set { isLock = value; } }
}

[System.Serializable]
public class Upgrade
{
    [SerializeField] float[] move_speed;
    [SerializeField] int[] move_speed_gold;
    [SerializeField] float[] hp;
    [SerializeField] int[] hp_gold;
    [SerializeField] float[] mp_plus;
    [SerializeField] int[] mp_plus_gold;
    [SerializeField] float[] skill_damage;
    [SerializeField] int[] skill_damage_gold;

    int move_speed_level = 0;
    int hp_level = 0;
    int mp_plus_level = 0;
    int skill_damage_level = 0;

    public bool MoveSpeedUpgrade(ref int gold)
    {
        if (move_speed_level >= 5 || move_speed_gold[move_speed_level] > gold)
        {
            AudioManager.Instance.PlaySFX("Button");
            return false;
        }
        gold -= move_speed_gold[move_speed_level];
        move_speed_level++;
        AudioManager.Instance.PlaySFX("UpgradeButton");
        return true;
    }

    public bool HPUpgrade(ref int gold)
    {
        if (hp_level >= 5 || hp_gold[hp_level] > gold)
        {
            AudioManager.Instance.PlaySFX("Button");
            return false;
        }

        gold -= hp_gold[hp_level];
        hp_level++;
        AudioManager.Instance.PlaySFX("UpgradeButton");
        return true;
    }

    public bool MPPlusUpgrade(ref int gold)
    {
        if (mp_plus_level >= 5 || mp_plus_gold[mp_plus_level] > gold)
        {
            AudioManager.Instance.PlaySFX("Button");
            return false;
        }
        gold -= mp_plus_gold[mp_plus_level];
        mp_plus_level++;
        AudioManager.Instance.PlaySFX("UpgradeButton");
        return true;
    }

    public bool SkillDamageUpgrade(ref int gold)
    {
        if (skill_damage_level >= 5 || skill_damage_gold[skill_damage_level] > gold)
        {
            AudioManager.Instance.PlaySFX("Button");
            return false;
        }
        gold -= skill_damage_gold[skill_damage_level];
        skill_damage_level++;
        AudioManager.Instance.PlaySFX("UpgradeButton");
        return true;
    }

    public float GetMoveSpeed() { return move_speed[move_speed_level]; }
    public int GetMoveSpeedLevel() { return move_speed_level; }
    public int GetMoveSpeedGold() { return move_speed_gold[move_speed_level]; }
    public float GetHP() { return hp[hp_level]; }
    public int GetHPLevel() { return hp_level; }
    public int GetHPGold() { return hp_gold[hp_level]; }
    public float GetMPPlus() { return mp_plus[mp_plus_level]; }
    public int GetMPPlusLevel() { return mp_plus_level; }
    public int GetMPPlusGold() { return mp_plus_gold[mp_plus_level]; }
    public float GetSkillDamage() { return skill_damage[skill_damage_level]; }
    public int GetSkillDamageLevel() { return skill_damage_level; }
    public int GetSkillDamageGold() { return skill_damage_gold[skill_damage_level]; }
    public void InitLevel(int move_speed, int hp, int mp_plus, int skill_damage)
    {
        move_speed_level = move_speed;
        hp_level = hp;
        mp_plus_level = mp_plus;
        skill_damage_level = skill_damage;
    }
}
