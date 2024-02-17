using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Monster
{
    public override void AttackReady()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        StartCoroutine(OnAttackReady());
    }

    IEnumerator OnAttackReady()
    {
        IsAttackReady = true;

        yield return new WaitForSeconds(data.AttackReadyTime);

        IsAttackReady = false;
        OnAttackState();
    }

    public override bool IsAttackBound()
    {
        float dist = DistanceToTarget();
        if (0 > dist)
            return false;
        return (dist <= agent.stoppingDistance);
    }

    public override void Attack()
    {
        base.Attack();
        anim.Attack();
        IsAttackAnimEnd = false;
    }
}
