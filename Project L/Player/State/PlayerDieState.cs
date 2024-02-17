using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Die State", menuName = "ScriptableObject/Player FSM State/Die", order = 7)]
public class PlayerDieState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.die;
        owner.Stop();
        owner.Die();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {

    }
}
