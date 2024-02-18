using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;


    public void Initialize()
    {
        if (anim)
        {
            anim.SetBool("IsMove",false);
            //anim.Play("Idle");
        }
    }

    public void Move()
    {
        anim.SetBool("IsMove", true);
    }

    public void Stop()
    {
        anim.SetBool("IsMove", false);
    }

    public void Charge()
    {
        anim.SetTrigger("Charge");
    }

    public void Shot()
    {
        anim.SetTrigger("Shot");
        //anim.SetFloat("Speed", TimeBetshot);
    }
}
