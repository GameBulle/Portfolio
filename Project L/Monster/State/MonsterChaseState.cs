using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase State", menuName = "ScriptableObject/Monster FSM State/Chase", order = 3)]
public class MonsterChaseState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.ChaseStart();
    }

    public void Excute(Monster owner)
    {
        if (owner.TargetLostCheck())
            owner.OnFindObjectState();
        else if (owner.IsAttackBound())
            owner.OnAttackReadyState();
        else
            owner.ChaseTarget();
    }

    public void Exit(Monster owner)
    {
        owner.ChaseEnd();
    }
}
