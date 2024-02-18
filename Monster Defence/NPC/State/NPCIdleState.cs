using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/NPC FSM State/Idle",order = 1)]
public class NPCIdleState : ScriptableObject,IState<NPC>
{
    public void Enter(NPC owner)
    {
        
    }

    public void Excute(NPC owner)
    {
        owner.AttackDurationCheck();

        if (owner.FindTarget())
        {
            if (owner.CheckShotAngle())
                owner.OnChargeState();
            else
                owner.OnMoveState();   
        }

    }

    public void Exit(NPC owner)
    {

    }
}
