using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Critical Attack State", menuName = "ScriptableObject/Boss FSM State/Giant Spider/Critical Attack", order = 3)]
public class GiantSpiderCriticalAttackState : ScriptableObject, IState<GiantSpider>
{
    public void Enter(GiantSpider owner)
    {
        owner.Stop();
        owner.AnimStop();
        owner.RoarAnim();
    }

    public void Excute(GiantSpider owner)
    {

    }

    public void Exit(GiantSpider owner)
    {
        owner.CriticalAttackEnd();
    }
}
