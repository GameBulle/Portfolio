using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evade State", menuName = "ScriptableObject/Player FSM State/Evade", order = 3)]
public class PlayerEvadeState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.evade;
        owner.Evade();
        owner.EvadeAnim();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {
        owner.EvadeStop();
    }
}
