using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "ScriptableObject/Monster FSM State/Attack", order = 4)]
public class MonsterAttackState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.Attack();
    }

    public void Excute(Monster owner)
    {
        
    }

    public void Exit(Monster owner)
    {

    }
}
