using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Move(Vector2 animVelocity, float reverseMaxWalk)
    {
        anim.SetFloat("VelocityX", animVelocity.x * reverseMaxWalk);
        anim.SetFloat("VelocityY", animVelocity.y * reverseMaxWalk);
    }

    public void Stop()
    {
        anim.SetFloat("VelocityX", 0);
        anim.SetFloat("VelocityY", 0);
    }

    public void Guard()
    {
        anim.SetBool("Guard", true);
    }

    public void GuardStop()
    {
        anim.SetBool("Guard", false);
    }

    public void GuardSuccess()
    {
        anim.SetTrigger("GuardSuccess");
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void Damaged()
    {
        anim.SetTrigger("Damaged");
    }

    public void Die()
    {
        anim.SetTrigger("Die");
    }

    public void GaurdBreak()
    {
        anim.SetTrigger("GuardBreak");
    }

    public void Evade(Vector3 dir)
    {
        anim.SetTrigger("Evade");
        anim.SetFloat("EvadeX", dir.x);
        anim.SetFloat("EvadeY", dir.y);
    }

    public void Skill()
    {
        anim.SetTrigger("Skill");
    }
    
    public void Execute()
    {
        anim.SetTrigger("Execute");
    }

    public void ChargeAttack()
    {
        anim.SetTrigger("ChargeAttack");
    }

    public void Rush()
    {
        anim.SetBool("Rush", true);
    }

    public void RushEnd()
    {
        anim.SetBool("Rush", false);
    }
}
