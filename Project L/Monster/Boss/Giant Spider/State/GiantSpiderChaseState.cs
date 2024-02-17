using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Chase", order = 4)]
public class GiantSpiderChaseState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.SetChaseTarget();
    }

    public void Excute(GiantSpider owner)
    {
        if (owner.IsAttackBound())
            owner.OnMeleeAttackState();
        else
            owner.ChaseTarget();
    }

    public void Exit(GiantSpider owner)
    {
        owner.ChaseTargetEnd();
    }
}
