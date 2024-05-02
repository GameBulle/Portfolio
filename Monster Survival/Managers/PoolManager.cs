using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Monster[] monster_prefabs;
    [SerializeField] Bullet[] bullet_prefabs;
    [SerializeField] Item[] item_prefabs;
    [SerializeField] Bullet[] skill_prefabs;

    List<Monster>[] monster_pools;
    List<Bullet>[] bullet_pools;
    List<Item>[] item_pools;
    List<Bullet>[] skill_pools;

    private void Awake()
    {
        Array.Sort(monster_prefabs, (a, b) => (a.ID < b.ID ? -1 : 1));
        monster_pools = new List<Monster>[monster_prefabs.Length];
        for (int i = 0; i < monster_pools.Length; i++)
            monster_pools[i] = new();

        bullet_pools = new List<Bullet>[bullet_prefabs.Length];
        for (int i = 0; i < bullet_pools.Length; i++)
            bullet_pools[i] = new();

        item_pools = new List<Item>[item_prefabs.Length];
        for (int i = 0; i < item_pools.Length; i++)
            item_pools[i] = new();

        skill_pools = new List<Bullet>[skill_prefabs.Length];
        for (int i = 0; i < skill_pools.Length; i++)
            skill_pools[i] = new();
    }

    public Monster GetMonster(int id)
    {
        Monster monster = null;

        foreach(Monster enemy in monster_pools[id])
        {
            if(!enemy.gameObject.activeSelf)
            {
                monster = enemy;
                break;
            }
        }

        if(monster == null)
        {
            monster = Instantiate(monster_prefabs[id],transform);
            monster_pools[id].Add(monster);
        }

        return monster;
    }

    public Bullet GetBullet(int index)
    {
        Bullet bullet = null;

        foreach(Bullet b in bullet_pools[index])
        {
            if(!b.gameObject.activeSelf)
            {
                bullet = b;
                break;
            }
        }

        if(bullet == null)
        {
            bullet = Instantiate(bullet_prefabs[index],transform);
            bullet_pools[index].Add(bullet);
        }

        return bullet;
    }

    public Item GetItem(int id)
    {
        Item item = null;
        foreach(Item i in item_pools[id])
        {
            if(!i.gameObject.activeSelf)
            {
                item = i;
                break;
            }
        }

        if(item == null)
        {
            item = Instantiate(item_prefabs[id],transform);
            item_pools[id].Add(item);
        }

        return item;
    }

    public Bullet GetSkill(int index)
    {
        Bullet bullet = null;

        foreach (Bullet skill in skill_pools[index])
        {
            if (!skill.gameObject.activeSelf)
            {
                bullet = skill;
                break;
            }
        }

        if (bullet == null)
        {
            bullet = Instantiate(skill_prefabs[index], transform);
            skill_pools[index].Add(bullet);
        }

        return bullet;
    }
}
