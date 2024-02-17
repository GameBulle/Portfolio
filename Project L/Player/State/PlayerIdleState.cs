using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/Player FSM State/Idle",order = 1)]
public class PlayerIdleState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.idle;
    }

    public void Excute(Player owner)
    {
        owner.StaminaPlus();
    }

    public void Exit(Player owner)
    {
        SoundMgr.Instance.StopMoveSound();
    }
}
