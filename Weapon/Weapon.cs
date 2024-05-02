using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Up { Damage, Speed, Scale }

    protected int id;
    protected WeaponData.SkillType type;
    protected int level;
    protected float damage;
    protected float speed;
    protected int per;
    protected int count;
    protected float knockback_power;

    protected float base_damage;
    protected float base_speed;
    protected int base_per;
    protected int base_count;
    protected float base_knockback_power;
    protected Vector3 scale;
    protected float shot_speed;

    public WeaponData.SkillType Type => type;
    public int ID => id;
    public int Level => level;

    public virtual void Initialize(WeaponData data)
    {
        level = 1;
        type = data.Skill;
        name = data.SkillName;
        transform.parent = GameManager.Instance.Player.transform.Find("Weapon").transform;
        transform.localPosition = Vector3.zero;

        id = (int)data.ID;
        base_damage = data.BaseDamage;
        base_speed = data.BaseSpeed;
        base_per = data.BasePer;
        base_count = data.BaseCount;
        base_knockback_power = data.BaseKnockback;
        AbilityApply();
        damage = base_damage;
        speed = base_speed;
        per = base_per;
        count = base_count;
        knockback_power = base_knockback_power;
        scale = Vector3.one;
        shot_speed = data.ShotSpeed;
    }

    public virtual void PassiveApply(Up type, float value)
    {
        switch (type)
        {
            case Up.Damage:
                damage += base_damage * value;
                break;
            case Up.Speed:
                SpeedUp(value);
                break;
            case Up.Scale:
                scale = Vector3.one + Vector3.one * value;
                break;
        }
        ArrangeWeapon();
    }

    void AbilityApply()
    {
        switch (type)
        {
            case SkillData.SkillType.Sword:
                base_damage *= CharacterAbility.Warrior * CharacterAbility.Shaman_Sword;
                break;
            case SkillData.SkillType.Magic:
                base_damage *= CharacterAbility.Wizard * CharacterAbility.Shaman_Magic;
                break;
        }
    }

    protected virtual void ArrangeWeapon() { }
    public virtual void LevelUp(float damage, float speed, int per, int count, float knockback)
    {
        level++;
        SpeedUp(speed);
    }
    protected virtual void Fire() { }
    protected virtual void SpeedUp(float value) { }
}
