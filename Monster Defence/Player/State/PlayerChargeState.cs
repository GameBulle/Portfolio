using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge State",menuName = "ScriptableObject/Player FSM State/Charge",order = 1)]
public class PlayerChargeState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        if (!owner.AttackDurationCheck())
        {
            owner.ChargeAnim();
            owner.Charge();
        }
        else
            owner.OnIdleState();
    }

    public void Excute(Player owner)
    {
        
    }

    public void Exit(Player owner)
    {

    }
}
