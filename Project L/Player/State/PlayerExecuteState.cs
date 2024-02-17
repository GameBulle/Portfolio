using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Execute State", menuName = "ScriptableObject/Player FSM State/Execute", order = 10)]
public class PlayerExecuteState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.Execute;
        owner.Execute();
        owner.ExecuteAnim();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {
        owner.ExecuteEnd();
    }
}
