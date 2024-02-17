using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "ScriptableObject/Player FSM State/Attack", order = 5)]
public class PlayerAttackState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.attack;
        owner.Stop();
        owner.Attack();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {

    }
}
