using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/Monster FSM State/Idle", order = 1)]
public class MonsterIdleState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.SetIdleTime();
    }
     
    public void Excute(Monster owner)
    {
        if (owner.Warning)
            owner.OnChaseState();
        else if (owner.CheckIdleTime())
            owner.OnWalkState();
        else if (owner.FindTarget())
            owner.OnFindObjectState();
    }

    public void Exit(Monster owner)
    {
        if (owner.Warning)
            owner.IdleToRun();
    }
}
