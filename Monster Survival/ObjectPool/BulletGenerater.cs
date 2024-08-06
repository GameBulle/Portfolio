using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerater : Generater<Bullet>
{
    [SerializeField] BulletFactory factory;

    public override void Initialize()
    {
        object_pools = new Queue<Bullet>[factory.Size];
        for (int i = 0; i < factory.Size; i++)
            object_pools[i] = new();
        factory.Initialize(this.transform);
    }

    public override Bullet GetObject(int id)
    {
        Bullet bullet = null;

        if (id == -1)
            id = factory.Size - 1;

        if (object_pools[id].Count == 0)
            object_pools[id].Enqueue(factory.CreateGameObject(id));

        bullet = object_pools[id].Dequeue();
        bullet.OnDestroy += ReturnBackPool;
        return bullet;
    }

    protected override void ReturnBackPool(Bullet ob)
    {
        ob.gameObject.SetActive(false);
        object_pools[(int)ob.ID].Enqueue(ob);
    }
}
