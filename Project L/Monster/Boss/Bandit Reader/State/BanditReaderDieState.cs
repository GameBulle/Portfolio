using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Die State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Die", order = 8)]
public class BanditReaderDieState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Die();
    }

    public void Excute(BanditReader owner)
    {

    }

    public void Exit(BanditReader owner)
    {

    }
}
