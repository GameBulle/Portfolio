using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Skill_AnimEvent : MonoBehaviour
{
    Collider2D[] colliders;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = false;
    }

    public void OnSmallCollider()
    {
        colliders[0].enabled = true;
    }

    public void OffSmallCollider()
    {
        colliders[0].enabled = false;
    }

    public void OnBigCollider()
    {
        colliders[1].enabled = true;
    }

    public void OffBigCollider()
    {
        colliders[1].enabled = false;
    }

    public void PlaySFX1()
    {
        AudioManager.Instance.PlaySFX("Wizard_Skill1");
    }

    public void PlaySFX2()
    {
        AudioManager.Instance.PlaySFX("Wizard_Skill2");
    }
}
