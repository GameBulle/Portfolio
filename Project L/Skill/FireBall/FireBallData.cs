using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBall Data", menuName = "ScriptableObject/Skill/FireBall", order = 1)]
public class FireBallData : ProjectileSkillData
{
    [SerializeField] FireBall fireBall;
    public override bool Use(Player player)
    {
        if(player.MP >= UseMP)
        {
            player.MP -= UseMP;
            FireBall ball = Instantiate(fireBall);
            ball.SetPosition(player.SkillPos);
            Vector3 dir = new Vector3(player.transform.forward.x, 0f, player.transform.forward.z);
            ball.Shot(dir, Level * Damage, Speed, Range);
            player.OnSkillState();
            SoundMgr.Instance.PlaySFXAudio("Skill");
            return true;
        }
        return false;
    }
}
