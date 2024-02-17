using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walk State", menuName = "ScriptableObject/Monster FSM State/Walk", order = 2)]
public class MonsterWalkState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.SetWalkTime();
        owner.GetRandomDir();
        owner.WalkAnim();
    }

    public void Excute(Monster owner)
    {
        if (owner.FindTarget())
            owner.OnIdleState();

        if (owner.CheckWalkTime())
            owner.OnIdleState();
    }

    public void Exit(Monster owner)
    {
        owner.Stop();
    }
}
