using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Battle", order = 2)]
public class GiantSpiderBattleState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.BattalAnim();
    }

    public void Excute(GiantSpider owner)
    {
        if (owner.CheckCriticalAttackDuration())
            owner.OnCriticalAttackState();
        else if (!owner.IsAttackBound())
            owner.OnChaseState();
        else
            owner.OnMeleeAttackState();
    }

    public void Exit(GiantSpider owner)
    {

    }
}
