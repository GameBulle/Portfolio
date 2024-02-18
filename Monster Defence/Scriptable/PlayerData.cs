using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateData : ScriptableObject
{
    public IState<Player> IdleState { get; private set; }
    public IState<Player> MoveState { get; private set; }
    public IState<Player> ChargeState { get; private set; }
    public IState<Player> ShotState { get; private set; }

    public void SetData(IState<Player> idle, IState<Player> move, IState<Player> charge, IState<Player> shot)
    {
        IdleState = idle;
        MoveState = move;
        ChargeState = charge;
        ShotState = shot;
    }
}