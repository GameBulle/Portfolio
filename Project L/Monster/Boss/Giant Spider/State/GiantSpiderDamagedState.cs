using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damaged State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Damaged", order = 8)]
public class GiantSpiderDamagedState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.Damaged();
    }

    public void Excute(GiantSpider owner)
    {

    }

    public void Exit(GiantSpider owner)
    {

    }
}
