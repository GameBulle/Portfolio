using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move State", menuName = "ScriptableObject/Monster FSM State/Move", order = 5)]
public class MonsterMoveState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.Move();
        owner.MoveAnim();
    }

    public void Excute(Monster owner)
    {
        
        if (owner.CheckAttackRange())
            owner.OnAttackState();
    }

    public void Exit(Monster owner)
    {
        owner.Stop();
        owner.StopAnim();
    }
}
