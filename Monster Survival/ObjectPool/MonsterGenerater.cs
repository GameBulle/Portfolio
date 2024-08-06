using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;

public class MonsterGenerater : Generater<Monster>
{
    [SerializeField] MonsterFactory factory;

    public override void Initialize()
    {
        object_pools = new Queue<Monster>[factory.Size];
        for (int i = 0; i < factory.Size; i++)
            object_pools[i] = new();
        factory.Initialize(this.transform);
    }

    public override Monster GetObject(int id)
    {
        Monster monster = null;

        if (object_pools[id].Count == 0)
            object_pools[id].Enqueue(factory.CreateGameObject(id));

        monster = object_pools[id].Dequeue();
        monster.OnDestroy += ReturnBackPool;
        return monster;
    }

    protected override void ReturnBackPool(Monster monster)
    {
        monster.gameObject.SetActive(false);
        object_pools[(int)monster.ID].Enqueue(monster);
    }
}
