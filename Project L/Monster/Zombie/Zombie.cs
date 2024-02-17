using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Monster
{
    [Header("Attack Effect")]
    [SerializeField] ParticleSystem[] attackEffect;

    public void Init()
    {
        Initialize();
        attackEffect[0].gameObject.SetActive(false);
        attackEffect[1].gameObject.SetActive(false);
    }

    public override void AttackReady()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        StartCoroutine(OnAttackReady());
    }

    IEnumerator OnAttackReady()
    {
        AttackEffectOn();
        IsAttackReady = true;

        yield return new WaitForSeconds(data.AttackReadyTime);

        AttackEffectOff();
        IsAttackReady = false;
        OnAttackState();
    }

    public void AttackEffectOn()
    {
        attackEffect[0].gameObject.SetActive(true);
        attackEffect[1].gameObject.SetActive(true);
    }

    public void AttackEffectOff()
    {
        attackEffect[0].gameObject.SetActive(false);
        attackEffect[1].gameObject.SetActive(false);
    }

    public override bool IsAttackBound()
    {
        {
            float dist = DistanceToTarget();
            if (0 > dist)
                return false;
            return (dist <= agent.stoppingDistance);
        }
    }

    public override void Attack()
    {
        base.Attack();
        anim.Attack();
        IsAttackAnimEnd = false;
    }
}
