using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCave : MonoBehaviour
{
    [SerializeField] int spawnMonsterID;
    [SerializeField] Transform spawnPos;
    [Header("SpawnTime x : min, y : max")]
    [SerializeField] Vector2 spawnTime;

    float time;

    private void Awake()
    {
        SetSpawnTime();
    }

    private void Update()
    {
        if(CheckSpawnTime())
        {
            MonsterMgr.Instance.SetMonsters(spawnMonsterID, spawnPos.position);
            SetSpawnTime();
        }
    }

    void SetSpawnTime()
    {
        time = UnityEngine.Random.Range(spawnTime.x, spawnTime.y);
    }

    bool CheckSpawnTime()
    {
        if (time <= 0)
            return true;

        time -= Time.deltaTime;
        return false;
    }
}
