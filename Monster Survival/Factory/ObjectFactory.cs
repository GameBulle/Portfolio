using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectFactory<T>
{
    [SerializeField] protected T[] prefabs;

    protected Transform[] object_container;
    //protected List<Transform> object_container;
    protected Transform parent_container;
    public int Size => prefabs.Length;

    public abstract T CreateGameObject(int id);
    public abstract void Initialize(Transform parent);
}
