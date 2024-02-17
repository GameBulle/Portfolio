using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Idle", order = 0)]
public class GiantSpiderIdleState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.SetIdleTime();
    }

    public void Excute(GiantSpider owner)
    {
        if (owner.FindTarget())
            owner.OnBattleState();
        else if(owner.CheckIdleTime())
            owner.OnWalkState();
    }

    public void Exit(GiantSpider owner)
    {

    }
}
