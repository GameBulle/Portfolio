using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMgr : Spawner<Monster>
{
    public enum MonsterType
    {
        Monster_Start = 0,
        Normal_Skeleton = Monster_Start,
        Knight_Skeleton,
        Shield_Skeleton,
        Slime,
        Dark_Warrior,
        Monsert_End
    }

    [SerializeField] float spawnTimeMin = 0.0f;
    [SerializeField] float spawnTimeMax = 0.0f;

    int[] monsterCount = null;
    List<int> containerIndex;

    public void Initialize()
    {
        gameObject.SetActive(true);
        System.Array.Sort(objectPools, (a, b) => (a.TargetObject.ID < b.TargetObject.ID) ? -1 : 1);

        monsterCount = new int[objectPools.Length];
        for (int i = 0; i < monsterCount.Length; i++)
            monsterCount[i] = 0;

        InitializeSpawner();
    }

    public int WaveStart()
    {
        containerIndex = new List<int>();
        for (int i = 0; i < objectPools.Length; i++)
            containerIndex.Add(i);

        SetMonsterCount();

        int totalMonster = 0;

        for (int i = 0; i < monsterCount.Length; i++)
            totalMonster += monsterCount[i];

        StartCoroutine(MonsterSpawn());

        return totalMonster;
    }

    public void WaveClear()
    {
        StopCoroutine(MonsterSpawn());
        
        ReturnPool();
    }

    void SetMonsterCount()
    {
        int wave = GameMgr.Instance.Wave;
        for(int i=(int)MonsterType.Monster_Start; i< (int)MonsterType.Monsert_End;i++)
        {
            switch(i)
            {
                case (int)MonsterType.Normal_Skeleton:
                    monsterCount[i] = wave * 3;
                    break;
                case (int)MonsterType.Knight_Skeleton:
                    monsterCount[i] = (int)Mathf.Floor(wave * 1.5f);
                    break;
                case (int)MonsterType.Shield_Skeleton:
                    monsterCount[i] = (int)Mathf.Floor(wave * 1.5f);
                    break;
                case (int)MonsterType.Slime:
                    monsterCount[i] = wave * 2;
                    break;
                case (int)MonsterType.Dark_Warrior:
                    monsterCount[i] = wave / 5;
                    break;
            }
        }
    }

    public Vector2 GetRandomPosition()
    {
        return new Vector2(14, UnityEngine.Random.Range(-5.0f, 2.5f));
    }

    IEnumerator MonsterSpawn()
    {
        while(true)
        {
            float spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
            yield return new WaitForSeconds(spawnTime);

            int index = 0;
            int cindex = 0;
            while (containerIndex.Count != 0)
            {
                cindex = Random.Range(0, containerIndex.Count);
                index = containerIndex[cindex];
                if (monsterCount[index] <= 0)
                {
                    containerIndex.RemoveAt(cindex);
                }
                else
                    break;
            }

            if (containerIndex.Count == 0)
                break;

            Monster monster = Spawn(index, GetRandomPosition());
            monster.Initialize();
            monster.SetData();
            monster.OnDieEvent += (monster) => GiveBackItem(monster);
            monsterCount[index]--;

        }
    }

    public void GameOver()
    {
        WaveClear();
        gameObject.SetActive(false);
    }
}
