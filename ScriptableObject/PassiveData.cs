using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveData", menuName = "Scriptable Object/PassiveData")]
public class PassiveData : SkillData
{
    public enum ApplicationType { Sword, Magic, Player }

    [SerializeField] ApplicationType application_type;

    [Header("## Level Data")]
    [SerializeField] float[] level_per_value;

    int level = 0;

    public ApplicationType Apply => application_type;
    public float LevelPerValue(int level) { return level_per_value[level]; }
    public int Level { get { return level; } set { level = value; } }
    public override Passive SetPassive()
    {
        GameObject ob = new GameObject();
        Passive passive = null;
        switch (skill_id)
        {
            case SkillID.Speed_Up:
                passive = ob.AddComponent<PlayerSpeedUp>();
                break;
            case SkillID.Get_Range_Up:
                passive = ob.AddComponent<PlayerGetRangeUp>();
                break;
            case SkillID.Health_Up:
                passive = ob.AddComponent<PlayerHealthUp>();
                break;
            case SkillID.Exp_Up:
                passive = ob.AddComponent<PlayerEXPUp>();
                break;
            case SkillID.MP_Up:
                passive = ob.AddComponent<PlayerMPUp>();
                break;
            case SkillID.Gold_Up:
                passive = ob.AddComponent<PlayerGoldUp>();
                break;
            case SkillID.Magic_Damage_Up:
                passive = ob.AddComponent<MagicDamageUp>();
                break;
            case SkillID.Magic_Speed_Up:
                passive = ob.AddComponent<MagicSpeedUp>();
                break;
            case SkillID.Magic_Scale_Up:
                passive = ob.AddComponent<MagicScaleUp>();
                break;
            case SkillID.Sword_Damage_Up:
                passive = ob.AddComponent<SwordDamageUp>();
                break;
            case SkillID.Sword_Speed_Up:
                passive = ob.AddComponent<SwordSpeedUp>();
                break;
            case SkillID.Sword_Scale_Up:
                passive = ob.AddComponent<SwordScaleUp>();
                break;
        }
        passive.Initialize(this);
        return passive;
    }
}
