using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shot State", menuName = "ScriptableObject/Player FSM State/Shot", order = 5)]
public class PlayerShotState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.ShotAnim();
        owner.LineRendererEnable();
        owner.Shot();
    }

    public void Excute(Player owner)
    {
        
    }

    public void Exit(Player owner)
    {

    }
}
