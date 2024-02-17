using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalAttack State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/NormalAttack", order = 5)]
public class BanditReaderNormalAttackState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.Attack();
    }

    public void Excute(BanditReader owner)
    {

    }

    public void Exit(BanditReader owner)
    {
        owner.NormalAttackEnd();
    }
}
