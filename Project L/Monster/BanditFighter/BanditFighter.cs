using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditFighter : Monster
{
    public override void AttackReady()
    {
        if (coroutine != null)
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
        int random = UnityEngine.Random.Range(0, 2);
        switch (random)
        {
            case 0:
                anim.Attack();
                break;
            case 1:
                anim.Attack2();
                break;
        }
        monsterAudio.PlaySFX(data.MonsterName + " Attack");
        IsAttackAnimEnd = false;
    }
}
