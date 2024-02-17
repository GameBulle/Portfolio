using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damaged State", menuName = "ScriptableObject/Player FSM State/Damaged", order = 6)]
public class PlayerDamagedState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.damaged;
        owner.Stop();
        owner.DamagedAnim();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {

    }
}
