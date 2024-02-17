using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditReaderAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Initialize()
    {
        Stop();
    }

    public void Stop()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
    }

    public void Walk()
    {
        anim.SetBool("Walk", true);
    }

    public void Battle()
    {
        anim.SetBool("Run", false);
        anim.SetTrigger("Battle");
    }

    public void Buff()
    {
        anim.SetTrigger("Buff");
    }

    public void Run()
    {
        anim.SetBool("Run", true);
    }

    public void NormalAttack()
    {
        anim.SetTrigger("NormalAttack");
    }

    public void KickAtack()
    {
        anim.SetTrigger("KickAttack");
    }

    public void ComboAttack()
    {
        anim.SetTrigger("ComboAttack");
    }

    public void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
    }

    public void GrogyEnd()
    {
        anim.SetTrigger("GrogyEnd");
    }

    public void Die()
    {
        anim.SetTrigger("Die");
    }

    public void Grogy()
    {
        anim.SetTrigger("Grogy");
    }
}
