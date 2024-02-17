using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Idle", order = 0)]
public class BanditReaderIdleState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.SetIdleTime();
    }

    public void Excute(BanditReader owner)
    {
        if (owner.FindTarget())
            owner.OnBattleState();
        else if (owner.CheckIdleTime())
            owner.OnWalkState();
    }

    public void Exit(BanditReader owner)
    {

    }
}
