using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMgr : Spawner<Monster>
{
    [SerializeField] MonsterBaseData[] monsters;
    [SerializeField] Boss[] bosses;

    static MonsterMgr instance = null;
    public static MonsterMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<MonsterMgr>();
                if(!instance)
                    instance = new GameObject("MonsterManager").AddComponent<MonsterMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    public void Initialize()
    {
        Array.Sort(monsters, (a, b) => (a.ID < b.ID) ? -1 : 1);
        InitializeSpawner();
    }

    public void SetMonsters(int mapID)
    {
        MapData currMap = MapMgr.Instance.GetMapData(mapID);
        for(int i=0;i<currMap.SpawnMonsterData.Length;i++)
        {
            Monster monster = Spawn(currMap.SpawnMonsterData[i].spawnMonsterID, currMap.SpawnMonsterData[i].spawnPos, currMap.SpawnMonsterData[i].spawnAngle);
            monster.gameObject.SetActive(true);
            monster.Initialize();
            monster.OnDieEvent += (monster) => GiveBackItem(monster);
        }
    }

    public void SetMonsters(int monsterID, Vector3 pos)
    {
        Monster monster = Spawn(monsterID, pos);
        monster.gameObject.SetActive(true);
        monster.Initialize();
        monster.OnDieEvent += (monster) => GiveBackItem(monster);
    }

    public void SetBoss(MP4Loader.Video video, Vector3 pos, Vector3 angle)
    {
        Boss boss = Instantiate(bosses[(int)video]);
        boss.gameObject.SetActive(true);
        boss.transform.position = pos;
        boss.transform.eulerAngles = angle;
        boss.Init();
    }

    public void AllRetunPool()
    {
        ReturnPool();
    }

    public MonsterBaseData GetMonsterData(int monsterID) 
    {
        return monsters[Array.FindIndex(monsters, a => a.ID == monsterID)]; 
    }
}
