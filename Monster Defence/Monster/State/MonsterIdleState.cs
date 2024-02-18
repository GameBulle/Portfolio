using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/Monster FSM State/Idle", order = 4)]
public class MonsterIdleState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {

    }

    public void Excute(Monster owner)
    {
        if (owner.isDead)
            owner.OnDieState();
        else
            owner.OnMoveState();
    }

    public void Exit(Monster owner)
    {

    }
}
