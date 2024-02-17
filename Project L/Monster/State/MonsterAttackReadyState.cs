using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Ready State", menuName = "ScriptableObject/Monster FSM State/Attack Ready", order = 7)]
public class MonsterAttackReadyState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {

    }

    public void Excute(Monster owner)
    {
        owner.LookRotationTarget();
        if (!owner.IsAttackReady)
        {
            if (!owner.IsAttackBound() && owner.IsAttackAnimEnd)
                owner.OnChaseState();
            else if (owner.CheckAttackDuration())
            {
                owner.AttackReady();
            }
                

        }
    }

    public void Exit(Monster owner)
    {
        owner.AttackReadyEnd();
    }
}
