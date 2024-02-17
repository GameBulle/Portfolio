using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSpiderAnimEvent : MonoBehaviour
{
    [SerializeField] Collider criticalAttackCollider;
    [SerializeField] Collider legAttackCollider;
    [SerializeField] Collider normalAttackCollider;
    [SerializeField] Transform legAttackEffectPos;
    [SerializeField] MonsterAudio bossAudio;

    public void Start()
    {
        criticalAttackCollider.gameObject.SetActive(false);
        legAttackCollider.gameObject.SetActive(false);
        normalAttackCollider.gameObject.SetActive(false);
    }

    public void CriticalAttack()
    {
        criticalAttackCollider.gameObject.SetActive(true);
    }

    public void CriticalAttackEnd()
    {
        EffectMgr.Instance.PlayParticleSystem("JumpDust", criticalAttackCollider.transform.position);
        bossAudio.PlaySFX("�Ŵ� �Ź� JumpAttack");
        criticalAttackCollider.gameObject.SetActive(false); 
    }

    public void LegAttack()
    {
        legAttackCollider.gameObject.SetActive(true);
    }

    public void LegAttackEnd()
    {
        EffectMgr.Instance.PlayParticleSystem("Dust", legAttackEffectPos.position);
        bossAudio.PlaySFX("�Ŵ� �Ź� LegAttack");
        legAttackCollider.gameObject.SetActive(false);
    }

    public void NormalAttack()
    {
        normalAttackCollider.gameObject.SetActive(true);
        bossAudio.PlaySFX("�Ŵ� �Ź� Attack");
    }

    public void NormalAttackEnd()
    {
        normalAttackCollider.gameObject.SetActive(false);
    }
}
