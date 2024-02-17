using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBall Data", menuName = "ScriptableObject/Skill/Chain Lightning",order = 2)]
public class ChainLightningData : ProjectileSkillData
{
    [SerializeField] ChainLightning chainLightning;
    [SerializeField] int chainCount;
    [SerializeField] float chainDelay;

    public int ChainCount => chainCount;

    public override bool Use(Player player)
    {
        if(player.MP >= UseMP)
        {
            player.MP -= UseMP;
            ChainLightning chain = Instantiate(chainLightning);
            chain.SetPosition(player.SkillPos);
            Vector3 dir = new Vector3(player.transform.forward.x, 0f, player.transform.forward.z);
            chain.Shot(dir, Level * Damage, Speed, Range, ChainCount, chainDelay);
            player.OnSkillState();
            SoundMgr.Instance.PlaySFXAudio("Skill");
            return true;
        }
        return false;
    }
}
