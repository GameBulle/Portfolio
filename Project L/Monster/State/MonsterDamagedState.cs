using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damaged State", menuName = "ScriptableObject/Monster FSM State/Damaged", order = 6)]
public class MonsterDamagedState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.Stop();
        owner.Damaged();
    }

    public void Excute(Monster owner)
    {
        if (owner.CheckStiffness())
            owner.OnIdleState();
    }

    public void Exit(Monster owner)
    {

    }
}
