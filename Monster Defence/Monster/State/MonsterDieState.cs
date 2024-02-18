using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Die State", menuName = "ScriptableObject/Monster FSM State/Die", order = 3)]
public class MonsterDieState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.DieAnim();
        owner.Die();
    }

    public void Excute(Monster owner)
    {
        
    }

    public void Exit(Monster owner)
    {

    }
}
