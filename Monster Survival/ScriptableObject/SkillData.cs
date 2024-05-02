using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public enum SkillType { Sword, Magic, Passive, Portion }
    public enum SkillID
    {
        Start, Wheel_Sword = 0, FireBall, Fire_Flame, Bolt_Breath, Bolt_Shot, Fire_Strike, Bolt_Ring, Throw_Dagger, Sword_Slash, Sword_Blow, Spinning_Sword, Dagger_Rampage, WeaponEnd = Dagger_Rampage,
        Speed_Up, Get_Range_Up, Health_Up, Exp_Up, MP_Up, Gold_Up, Magic_Damage_Up, Magic_Speed_Up, Magic_Scale_Up, Sword_Damage_Up, Sword_Speed_Up, Sword_Scale_Up, PassiveEnd = Sword_Scale_Up, Portion
    }

    [SerializeField] SkillType skill_type;
    [SerializeField] protected SkillID skill_id;
    [SerializeField] Sprite icon;
    [SerializeField] string skill_name;
    [TextArea]
    [SerializeField] string skill_desc;

    public SkillType Skill => skill_type;
    public SkillID ID => skill_id;
    public Sprite Icon => icon;
    public string SkillName => skill_name;
    public string SkillDesc => skill_desc;
    public virtual Weapon SetWeapon() { return null; }
    public virtual Passive SetPassive() {  return null; }
}
