using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] MonsterGenerater monster_generater;
    [SerializeField] ItemGenerater item_generater;
    [SerializeField] BulletGenerater bullet_generater;

    public void GameStart()
    {
        monster_generater.Initialize();
        item_generater.Initialize();
        bullet_generater.Initialize();
    }

    public Monster GetMonster(int id) { return monster_generater.GetObject(id); }
    public Item GetItem(int id) { return item_generater.GetObject(id); }
    public Bullet GetBullet(int id) { return bullet_generater.GetObject(id); }
}
