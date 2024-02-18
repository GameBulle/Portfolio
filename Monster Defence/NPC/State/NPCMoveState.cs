using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move State", menuName = "ScriptableObject/NPC FSM State/Move", order = 2)]
public class NPCMoveState : ScriptableObject, IState<NPC>
{
    public void Enter(NPC owner)
    {
        owner.MoveAnim();
    }

    public void Excute(NPC owner)
    {
        owner.AttackDurationCheck();

        if (owner.CheckMove())
            owner.OnIdleState();
        else
        {
            if (owner.FindTarget())
                owner.Move();
            else
                owner.OnIdleState();
        }
    }

    public void Exit(NPC owner)
    {
        owner.Stop();
        owner.StopAnim();
    }
}
