using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Attack State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Melee Attack", order = 5)]
public class GiantSpiderMeleeAttackState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.LookTarget();
        if (owner.CheckLegAttackDuration())
            owner.LegAttack();
        else if (owner.CheckNormalAttackDuration())
            owner.Attack();
        else
            owner.OnBattleState();
    }

    public void Excute(GiantSpider owner)
    {

    }

    public void Exit(GiantSpider owner)
    {
        owner.AttackEnd();
        owner.LegAttackEnd();
    }
}
