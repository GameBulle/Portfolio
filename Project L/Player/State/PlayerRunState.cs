using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Run State", menuName = "ScriptableObject/Player FSM State/Run", order = 2)]
public class PlayerRunState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.run;
        SoundMgr.Instance.Run(true);
    }

    public void Excute(Player owner)
    {
        owner.StaminaMinus();
    }

    public void Exit(Player owner)
    {
        SoundMgr.Instance.Run(false);
        SoundMgr.Instance.StopMoveSound();
    }
}
