using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool<T> where T : MonoBehaviour,IPoolingObject
{
    [SerializeField] T targetObject;

    Queue<T> objectPool;
    int PoolingAmount = 1;
    Transform containerObject;

    public T TargetObject => targetObject;

    public bool Initialize()
    {
        if (!targetObject || containerObject)
            return false;

        if (1 > PoolingAmount)
            PoolingAmount = 1;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("Object Pool Container : ");
        sb.Append(targetObject.name);
        containerObject = new GameObject(sb.ToString()).transform;
        objectPool = new Queue<T>();
        return true;
    }

    bool MakeAndPooling()
    {
        if (!containerObject)
            return false;

        T poolObject;
        for (int i = 0; PoolingAmount > i; i++) 
        {
            poolObject = MonoBehaviour.Instantiate(targetObject, containerObject);
            poolObject.name = targetObject.name;
            poolObject.gameObject.SetActive(false);
            objectPool.Enqueue(poolObject);
        }
        return true;
    }

    public bool GetObject(out T Object)
    {
        Object = null;

        if (!containerObject)
            return false;

        if(0 == objectPool.Count)
        {
            if (!MakeAndPooling())
                return false;   
        }

        Object = objectPool.Dequeue();
        Object.gameObject.SetActive(true);
        return true;
    }

    public bool CheckObject(T Object)
    {
        if (!targetObject)
            return false;
        return targetObject.name.Equals(Object.name);
    }

    public bool PutInPool(T Object)
    {
        if (!(Object && containerObject))
            return false;

        Object.ReturnBack();
        Object.gameObject.SetActive(false);
        objectPool.Enqueue(Object);
        return true;
    }

    public bool Destroy()
    {
        if (!containerObject)
            return false;

        MonoBehaviour.Destroy(containerObject.gameObject);
        containerObject = null;

        objectPool.Clear();
        objectPool = null;
        return true;
    }

    public void ReturnBackPool()
    {
        if(containerObject)
        {
            foreach(Transform child in containerObject)
            {
                if(child.gameObject.activeSelf)
                {
                    if (child.TryGetComponent(out T Object))
                        PutInPool(Object);
                }
            }
        }
    }
}
