using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damaged State", menuName = "ScriptableObject/Monster FSM State/Damaged", order = 2)]
public class MonsterDamagedState : ScriptableObject, IState<Monster>
{
    public void Enter(Monster owner)
    {
        owner.DamagedAnim();
    }

    public void Excute(Monster owner)
    {
        owner.AttackDurationCheck();
        if (owner.CheckStiffnessTime())
            return;
    }

    public void Exit(Monster owner)
    {
        
    }
}
