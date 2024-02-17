using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Initialize()
    {
        Stop();
    }

    public void Walk()
    {
        anim.SetBool("walk", true);
    }

    public void Run()
    {
        anim.SetBool("run", true);
    }

    public void Attack()
    {
        anim.SetTrigger("attack");
    }

    public void Hit()
    {
        anim.SetTrigger("hit");
    }

    public void Die()
    {
        anim.SetTrigger("die");
    }

    public void IdleToRun()
    {
        anim.SetTrigger("idleTorun");
    }

    public void Stop()
    {
        anim.SetBool("walk", false);
        anim.SetBool("run", false);
    }

    public void LookAround()
    {
        anim.SetBool("findObject", true);
    }

    public void LookAroundStop()
    {
        anim.SetBool("findObject", false);
    }

    public void Walk_Back()
    {
        anim.SetBool("walkback", true);
    }

    public void Walk_BackStop()
    {
        anim.SetBool("walkback", false);
    }

    public void Charge()
    {
        anim.SetTrigger("charge");
    }

    public void Executed()
    {
        anim.SetTrigger("Executed");
    }

    public void Attack2()
    {
        anim.SetTrigger("attack2");
    }
}
