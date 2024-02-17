using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damaged State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Damaged", order = 10)]
public class BanditReaderDamagedState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Damaged();
    }

    public void Excute(BanditReader owner)
    {

    }

    public void Exit(BanditReader owner)
    {

    }
}
