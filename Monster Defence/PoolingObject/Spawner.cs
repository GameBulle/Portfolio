using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour,IPoolingObject
{
    [Header("Object Pool")]
    [SerializeField] protected ObjectPool<T>[] objectPools = null;

    public int PoolCount => objectPools.Length;
    public int SpawnCount { get; private set; } = 0;

    protected void InitializeSpawner()
    {
        int count = objectPools.Length;
        for (int i = 0; count > i; i++)
            objectPools[i].Initialize();
    }

    public T Spawn(int index, Vector2 center)
    {
        if (0 > index || (objectPools.Length - 1) < index)
            return null;

        if (objectPools[index].GetObject(out T item))
        {
            Vector2 pos = center;
            item.SetPosition(pos);
            SpawnCount++;
            return item;
        }
        return null;
    }

    public bool GiveBackItem(T item)
    {
        int count = objectPools.Length;
        for (int i = 0; count > i; i++)
        {
            if (objectPools[i].CheckObject(item))
            {
                objectPools[i].PutInPool(item);
                SpawnCount--;
                return true;
            }
        }
        return false;
    }

    public void ReturnPool()
    {
        int count = objectPools.Length;
        for (int i = 0; count > i; i++)
        {
            objectPools[i].ReturnBackPool();
        }
        SpawnCount = 0;
    }
}
