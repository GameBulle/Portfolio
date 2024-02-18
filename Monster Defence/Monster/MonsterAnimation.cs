using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Initialize()
    {
        if(anim)
        {
            anim.SetBool("IsMove", false);
        }
    }

    public void Move()
    {
        //Debug.Log("MoveAnim");
        anim.SetBool("IsMove", true);
    }

    public void Stop()
    {
        anim.SetBool("IsMove", false);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void Damaged()
    {
        anim.SetTrigger("Hit");
    }

    public void Die()
    {
        anim.SetTrigger("Die");
    }
}
