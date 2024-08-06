using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerater : Generater<Item>
{
    [SerializeField] ItemFactory factory;

    public override void Initialize()
    {
        object_pools = new Queue<Item>[factory.Size];
        for (int i = 0; i < factory.Size; i++)
            object_pools[i] = new();
        factory.Initialize(this.transform);
    }

    public override Item GetObject(int id)
    {
        Item item = null;

        if (object_pools[id].Count == 0)
            object_pools[id].Enqueue(factory.CreateGameObject(id));

        item = object_pools[id].Dequeue();
        item.OnDestroy += ReturnBackPool;
        return item;
    }

    protected override void ReturnBackPool(Item ob)
    {
        ob.gameObject.SetActive(false);
        object_pools[(int)ob.ID].Enqueue(ob);
    }
}
