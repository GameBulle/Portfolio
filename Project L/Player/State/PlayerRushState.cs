using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rush State", menuName = "ScriptableObject/Player FSM State/Rush", order = 13)]
public class PlayerRushState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        //owner.act = Player.Act.Rush;
        owner.Rush();
        owner.RushAnim();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {
        owner.RushEnd();
    }
}
