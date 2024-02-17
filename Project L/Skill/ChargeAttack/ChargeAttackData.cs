using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge Attack Data", menuName = "ScriptableObject/Skill/Charge Attack", order = 3)]
public class ChargeAttackData : SkillData
{
    [SerializeField] float attackDistance;

    public float AttackDistance => attackDistance;

    public override bool Use(Player player)
    {
        if (player.MP >= UseMP)
        {
            player.MP -= UseMP;
            player.OnChargeState();
            return true;
        }
        return false;
    }
}
