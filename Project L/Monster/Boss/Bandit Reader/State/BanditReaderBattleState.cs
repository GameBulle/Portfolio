using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Battle", order = 2)]
public class BanditReaderBattleState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.BattleAnim();
    }

    public void Excute(BanditReader owner)
    {
        if (owner.CheckBuffCoolTime())
            owner.OnBuffState(); 
        else if (owner.IsJumpAttackBound() && owner.CheckJumpAttackDuration())
            owner.OnJumpAttackState();
        else if (!owner.IsAttackBound())
            owner.OnChaseState();
        else if (owner.CheckComboAttackDuration())
            owner.OnComboAttackState();
        else
            owner.OnNormalAttackState();
    }

    public void Exit(BanditReader owner)
    {

    }
}
