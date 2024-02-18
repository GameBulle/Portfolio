using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge State", menuName = "ScriptableObject/NPC FSM State/Charge",order = 4)]
public class NPCChargeState : ScriptableObject,IState<NPC>
{
    public void Enter(NPC owner)
    {
        if (!owner.AttackDurationCheck())
        {
            owner.ChargeAnim();
            owner.Charge(1);
        }
        else
            owner.OnIdleState();
    }

    public void Excute(NPC owner)
    {
        if (owner.CheckShot())
            owner.OnShotState();
    }

    public void Exit(NPC owner)
    {
        
    }
}
