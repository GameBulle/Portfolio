using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/Player FSM State/Idle", order = 2)]
public class PlayerIdleState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.Stop();
        owner.StopAnim();
    }

    public void Excute(Player owner)
    {
        owner.AttackDurationCheck();
    }

    public void Exit(Player owner)
    {

    }
}
