using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaEvent : MonoBehaviour
{
    [Header("Attack Collider")]
    [SerializeField] Collider attack;
    [SerializeField] ParticleSystem[] meleeEffect;

    [Header("Charge Attack Effect")]
    [SerializeField] Transform chargeAttackPos;


    private void Start()
    {
        attack.gameObject.SetActive(false);
    }

    public void MeleeEffect()
    {
        meleeEffect[0].gameObject.SetActive(true);
        meleeEffect[1].gameObject.SetActive(true);
    }

    public void AttackEffectOff()
    {
        meleeEffect[0].gameObject.SetActive(false);
        meleeEffect[1].gameObject.SetActive(false);
    }

    public void AttackColliderActive(int isPlayer = 0)
    {
        attack.gameObject.SetActive(true);
        if(isPlayer == 1)
            SoundMgr.Instance.PlaySFXAudio("Attack_Swing");
    }

    public void AttackColliderDisabled()
    {
        attack.gameObject.SetActive(false);
    }

    public void ChargeAttackEffect()
    {
        EffectMgr.Instance.PlayParticleSystem("ChargeAttack", chargeAttackPos.position);
        SoundMgr.Instance.PlaySFXAudio("ObstacleExplosion");
    }
}
