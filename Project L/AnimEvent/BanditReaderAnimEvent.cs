using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BanditReaderAnimEvent : MonoBehaviour
{
    [SerializeField] Collider[] comboAttacks;
    [SerializeField] GameObject[] comboAttackRanges;
    [SerializeField] Collider jumpAttack;
    [SerializeField] Collider normalAttack;
    [SerializeField] MonsterAudio bossAudio;

    private void Start()
    {
        for(int i=0;i<comboAttacks.Length;i++)
        {
            comboAttacks[i].gameObject.SetActive(false);
            comboAttackRanges[i].SetActive(false);
        }

        jumpAttack.gameObject.SetActive(false);
        normalAttack.gameObject.SetActive(false);
    }

    public void ComboAttack1Range() { comboAttackRanges[0].SetActive(true); }
    public void ComboAttack1Collider() 
    { 
        comboAttacks[0].gameObject.SetActive(true);
        bossAudio.PlaySFX("魂利 滴格" + " Attack");
    }

    public void ComboAttack1End()
    {
        comboAttackRanges[0].SetActive(false);
        comboAttacks[0].gameObject.SetActive(false);
    }

    public void ComboAttack3Range() { comboAttackRanges[1].SetActive(true); }
    public void ComboAttack3Collider() { comboAttacks[1].gameObject.SetActive(true); bossAudio.PlaySFX("魂利 滴格" + " Attack"); }
    public void ComboAttack3End()
    {
        comboAttackRanges[1].SetActive(false);
        comboAttacks[1].gameObject.SetActive(false);
    }

    public void JumpAttackCollider() { jumpAttack.gameObject.SetActive(true); }
    public void JumpAttackEnd()
    {
        jumpAttack.gameObject.SetActive(true);
    }

    public void NormalAttack() { normalAttack.gameObject.SetActive(true); }
    public void NormalAttackEnd() { normalAttack.gameObject.SetActive(false); }

    public void Taunt()
    {
        GameMgr.Instance.ShakeCamera(1.5f, 0.6f);
    }

    public void JumpAttack()
    {
        GameMgr.Instance.ShakeCamera(0.8f, 2f);
    }
}
