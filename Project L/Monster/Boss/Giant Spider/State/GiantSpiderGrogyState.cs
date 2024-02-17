using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grogy State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Grogy", order = 6)]
public class GiantSpiderGrogyState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.Grogy();
    }

    public void Excute(GiantSpider owner)
    {

    }

    public void Exit(GiantSpider owner)
    {

    }
}
