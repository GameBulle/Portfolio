using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Shot State", menuName = "ScriptableObject/NPC FSM State/Shot", order = 3)] 
public class NPCShotState : ScriptableObject, IState<NPC>
{
    public void Enter(NPC owner)
    {
        owner.ShotAnim();
        owner.Shot();
    }

    public void Excute(NPC owner)
    {
        
    }

    public void Exit(NPC owner)
    {

    }
}
