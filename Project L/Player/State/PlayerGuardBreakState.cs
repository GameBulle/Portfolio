using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuardBreak State", menuName = "ScriptableObject/Player FSM State/GuardBreak", order = 8)]
public class PlayerGuardBreakState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.guardBreak;
        owner.GuardBreak();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {
        owner.GuardBreakEnd();
    }
}
