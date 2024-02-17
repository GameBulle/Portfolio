using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Die State", menuName = "ScriptableObject/Monster FSM State/Die", order = 5)]
public class MonsterDieState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.Die();
        owner.DieAnim();
    }

    public void Excute(Monster owner)
    {

    }

    public void Exit(Monster owner)
    {

    }
}
