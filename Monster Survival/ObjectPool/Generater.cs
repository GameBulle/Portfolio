using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generater<T> : MonoBehaviour where T : MonoBehaviour, IPoolingObject<T>
{
    protected Queue<T>[] object_pools;

    public abstract void Initialize();
    public abstract T GetObject(int id);
    protected abstract void ReturnBackPool(T ob);
}
