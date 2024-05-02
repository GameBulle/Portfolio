using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Object/WeaponData")]
public class WeaponData : SkillData
{
    [Header("## Level Data")]
    [SerializeField] float shot_speed;
    [SerializeField] float base_damage;
    [SerializeField] float base_speed;
    [SerializeField] int base_per;
    [SerializeField] int base_count;  
    [SerializeField] float base_knockback;
    [SerializeField] float[] level_per_damage;
    [SerializeField] float[] level_per_speed;
    [SerializeField] int[] level_per_per;
    [SerializeField] int[] level_per_count;
    [SerializeField] float[] level_per_knockback;

    public float ShotSpeed => shot_speed;
    public float BaseDamage => base_damage;
    public float BaseSpeed => base_speed;
    public int BasePer => base_per;
    public int BaseCount => base_count;
    public float BaseKnockback => base_knockback;
    public float LevelPerDamage(int level) { return level_per_damage[level]; }
    public float LevelPerSpeed(int level) { return level_per_speed[level]; }
    public int LevelPerPer(int level) { return level_per_per[level]; }
    public int LevelPerCount(int level) { return level_per_count[level]; }
    public float LevelPerKnockback(int level) { return level_per_knockback[level]; }
    public override Weapon SetWeapon()
    {
        GameObject ob = new GameObject();
        Weapon weapon = null;
        switch(skill_id)
        {
            case SkillID.Wheel_Sword:
                weapon = ob.AddComponent<Wheel_Sword>();
                break;
            case SkillID.FireBall:
                weapon = ob.AddComponent<Fire_Ball>();
                break;
            case SkillID.Fire_Flame:
                weapon = ob.AddComponent<Fire_Flame>();
                break;
            case SkillID.Bolt_Breath:
                weapon = ob.AddComponent<Bolt_Breath>();
                break;
            case SkillID.Bolt_Shot:
                weapon = ob.AddComponent<Bolt_Shot>();
                break;
            case SkillID.Fire_Strike:
                weapon = ob.AddComponent<Fire_Strike>();
                break;
            case SkillID.Bolt_Ring:
                weapon = ob.AddComponent<Bolt_Ring>();
                break;
            case SkillID.Throw_Dagger:
                weapon = ob.AddComponent<Throw_Dagger>();
                break;
            case SkillID.Sword_Slash:
                weapon = ob.AddComponent<Sword_Slash>();
                break;
            case SkillID.Sword_Blow:
                weapon = ob.AddComponent<Sword_Blow>();  
                break;
            case SkillID.Spinning_Sword:
                weapon = ob.AddComponent<Spinning_Sword>();
                break;
            case SkillID.Dagger_Rampage:
                weapon = ob.AddComponent<Dagger_Rampage>();
                break;  
        }
        weapon.Initialize(this);
        return weapon;
    }
}
