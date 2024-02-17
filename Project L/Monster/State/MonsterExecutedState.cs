using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Executed State", menuName = "ScriptableObject/Monster FSM State/Executed", order = 10)]
public class MonsterExecutedState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.Executed();
        owner.ExecutedAnim();
    }

    public void Excute(Monster owner)
    {

    }

    public void Exit(Monster owner)
    {

    }
}
