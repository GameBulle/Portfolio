using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge Attack State", menuName = "ScriptableObject/Player FSM State/Charge Attack", order = 12)]
public class PlayerChargeAttackState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.ChargeAttack;
        owner.ChargeAttack();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {
        owner.ChargeAttackEnd();
    }
}
