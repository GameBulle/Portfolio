using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Find Object State", menuName = "ScriptableObject/Monster FSM State/Find Object", order = 8)]
public class MonsterFindObjectState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.LookAround();
    }

    public void Excute(Monster owner)
    {
        owner.FindTarget();

        if (!owner.IsThereTarget)
            owner.OnIdleState();
        else if (owner.Warning)
            owner.OnChaseState();  
    }

    public void Exit(Monster owner)
    {
        owner.LookAroundStop();
    }
}
