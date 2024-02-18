using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "ScriptableObject/Monster FSM State/Attack", order = 1)]
public class MonsterAttackState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {

    }

    public void Excute(Monster owner)
    {
        if (owner.AttackDurationCheck())
            return;

        owner.AttackAnim();
        owner.Attack();
    }

    public void Exit(Monster owner)
    {

    }
}
