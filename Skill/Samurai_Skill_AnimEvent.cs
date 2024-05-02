using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Samurai_Skill_AnimEvent : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    public void OnCollider()
    {
        coll.enabled = true;
    }

    public void OffCollider()
    {
        coll.enabled = false;
    }

    public void PlaySFX1()
    {
        AudioManager.Instance.PlaySFX("Samurai_Skill1");
    }

    public void PlaySFX2()
    {
        AudioManager.Instance.PlaySFX("Samurai_Skill2");
    }
}
