using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walk State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Walk", order = 1)]
public class GiantSpiderWalkState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.SetWalkTime();
        owner.GetRandomDir();
        owner.WalkAnim();
    }

    public void Excute(GiantSpider owner)
    {
        if (owner.FindTarget())
            owner.OnBattleState();
        else if (owner.CheckWalkTime())
            owner.OnIdleState();
    }

    public void Exit(GiantSpider owner)
    {

    }
}
