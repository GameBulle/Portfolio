using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Chase", order = 4)]
public class BanditReaderChaseState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.SetChaseTarget();
    }

    public void Excute(BanditReader owner)
    {
        if (owner.IsAttackBound())
            owner.OnBattleState();
        else
            owner.ChaseTarget();
    }

    public void Exit(BanditReader owner)
    {

    }
}
