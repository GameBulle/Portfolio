using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : Character
{
    void Enter(T owner);
    void Excute(T owner);
    void Exit(T owner);
}

public class FSM<T> where T : Character
{
    T owner;
    IState<T> currState = null;
    IState<T> prevState = null;
    public IState<T> PrevState { get { return prevState; } set { prevState = value; } }

    FSM() { }
    public FSM(T owner) { this.owner = owner; }
    public void Update() { currState.Excute(owner); }
    public bool SetCurrState(IState<T> state)
    {
        if (state == null)
            return false;
        currState = state;
        currState.Enter(owner);
        return true;
    }

    public bool ChangeState(IState<T> state)
    {
        if (state == null)
            return false;
        currState.Exit(owner);
        currState = state;
        currState.Enter(owner);
        return true;
    }
}
