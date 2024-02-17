using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Die State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Die", order = 7)]
public class GiantSpiderDieState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.Die();
    }

    public void Excute(GiantSpider owner)
    {

    }

    public void Exit(GiantSpider owner)
    {

    }
}
