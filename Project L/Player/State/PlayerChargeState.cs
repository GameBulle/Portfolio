using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge State", menuName = "ScriptableObject/Player FSM State/Charge", order = 11)]
public class PlayerChargeState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.Charge;
        owner.Charge();
        owner.ChargeAnim();
    }

    public void Excute(Player owner)
    {
        
    }

    public void Exit(Player owner)
    {

    }
}
