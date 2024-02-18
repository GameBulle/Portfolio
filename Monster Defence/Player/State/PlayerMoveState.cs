using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Move State", menuName = "ScriptableObject/Player FSM State/Move", order = 3)]
public class PlayerMoveState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        SoundMgr.Instance.MoveSoundPlay();
        owner.MoveAnim();
    }

    public void Excute(Player owner)
    {
        owner.AttackDurationCheck();
        owner.Move();
    }

    public void Exit(Player owner)
    {
        SoundMgr.Instance.MoveSoundStop();
    }
}
