using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo Attack State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/ComboAttack", order = 7)]
public class BanditReaderComboAttackState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.ComboAttack();
    }

    public void Excute(BanditReader owner)
    {

    }

    public void Exit(BanditReader owner)
    {
        owner.ComboAttackEnd();
    }
}
