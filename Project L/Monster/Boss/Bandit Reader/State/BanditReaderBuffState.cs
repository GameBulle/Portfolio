using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Buff", order = 3)]
public class BanditReaderBuffState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.Buff();
    }

    public void Excute(BanditReader owner)
    {

    }

    public void Exit(BanditReader owner)
    {

    }
}
