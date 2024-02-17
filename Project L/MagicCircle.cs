using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : ActionObject
{
    [SerializeField] GameObject obstacle;
    [SerializeField] Transform explosionPos;
    [SerializeField] int spawnMonsterID;
    [SerializeField] Transform[] monsterSpawnPos;
    public override void Action()
    {
        SoundMgr.Instance.PlaySFXAudio("Action");
        MagicDoor door = FindObjectOfType<MagicDoor>();
        door.ReleaseBarrier(InteractionName);

        SoundMgr.Instance.PlaySFXAudio("ObstacleExplosion");
        EffectMgr.Instance.PlayParticleSystem("ObstacleExplosion", explosionPos.position);
        GameMgr.Instance.ShakeCamera(1.0f, 1.5f);
        obstacle.gameObject.SetActive(false);
        for (int i = 0; i < monsterSpawnPos.Length; i++)
            MonsterMgr.Instance.SetMonsters(spawnMonsterID, monsterSpawnPos[i].position);

        RemoveInteractionToList();
        gameObject.SetActive(false);
    }
}
