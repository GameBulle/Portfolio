using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour,IPoolingObject
{
    [Header("Object Pool")]
    [SerializeField] protected ObjectPool<T>[] objectPools = null;

    public int PoolCount => objectPools.Length;
    public int SpawnCount { get; private set; }

    protected void InitializeSpawner()
    {
        for (int i = 0; i < PoolCount; i++)
            objectPools[i].Initialize();
        SpawnCount = 0;
    }

    public T Spawn(int index, Vector3 center = default(Vector3), Vector3 angle = default(Vector3))
    {
        if (objectPools[index].GetObject(out T Object))
        {
            Vector3 pos = center;
            Object.SetPosition(pos);
            Object.SetAngle(angle);
            SpawnCount++;
            return Object;
        }
        return null;
    }

    public bool GiveBackItem(T item)
    {
        for (int i = 0; PoolCount > i; i++)
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
        for (int i = 0; PoolCount > i; i++)
            objectPools[i].ReturnBackPool();
        SpawnCount = 0;
    }
}
