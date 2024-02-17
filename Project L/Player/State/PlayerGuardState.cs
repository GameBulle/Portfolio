using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Guard State", menuName = "ScriptableObject/Player FSM State/Guard", order = 4)]
public class PlayerGuardState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.guard;
        owner.GuardOn();
    }

    public void Excute(Player owner)
    {
        owner.StaminaMinus();
        if (owner.CheckStaminaGuard)
            owner.OnIdleState();
    }

    public void Exit(Player owner)
    {
        owner.GuardOff();
    }
}
