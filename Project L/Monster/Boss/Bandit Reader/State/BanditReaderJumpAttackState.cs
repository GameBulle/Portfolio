using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpAttack State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/JumpAttack", order = 6)]
public class BanditReaderJumpAttackState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.LookTarget();
        owner.JumpAttack();
    }

    public void Excute(BanditReader owner)
    {

    }

    public void Exit(BanditReader owner)
    {

    }
}
