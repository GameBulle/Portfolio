using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSpiderAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Initialize()
    {
        Stop();
    }

    public void Walk()
    {
        anim.SetBool("Walk", true);
    }

    public void Stop()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
    }

    public void Roar()
    {
        anim.SetTrigger("Roar");
    }

    public void CriticalAttack()
    {
        anim.SetTrigger("CriticalAttack");
    }

    public void Run()
    {
        anim.SetBool("Run", true);
    }

    public void NormalAttack()
    {
        anim.SetTrigger("NormalAttack");
    }

    public void LegAttack()
    {
        anim.SetTrigger("LegAttack");
    }

    public void Grogy()
    {
        anim.SetTrigger("Grogy");
    }

    public void GrogyEnd()
    {
        anim.SetTrigger("GrogyEnd");
    }

    public void Die()
    {
        anim.SetTrigger("Die");
    }

    public void Damaged()
    {
        anim.SetTrigger("Damaged");
    }

    public void Battle()
    {
        anim.SetTrigger("Battle");
    }
}
