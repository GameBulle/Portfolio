using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walk State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Walk", order = 1)]
public class BanditReaderWalkState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.SetWalkTime();
        owner.GetRandomDir();
        owner.WalkAnim();
    }

    public void Excute(BanditReader owner)
    {
        if (owner.FindTarget())
            owner.OnBattleState();
        else if (owner.CheckWalkTime())
            owner.OnIdleState();
    }

    public void Exit(BanditReader owner)
    {

    }
}
