using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ObjectPool<T> where T : MonoBehaviour,IPoolingObject
{
    [SerializeField] T targetObject;
    [SerializeField] Transform parentContainer;
    Transform containerObject;

    Queue<T> objectPool;

    public T TargetObject => targetObject;

    public void Initialize()
    {
        StringBuilder sb = new();
        sb.Append("Object Pool Container : ");
        sb.Append(targetObject.name);
        objectPool = new();
        containerObject = new GameObject(sb.ToString()).transform;
        containerObject.SetParent(parentContainer);
    }

    bool MakeAndPooling()
    {
        if (!containerObject)
            return false;

        T poolObject = MonoBehaviour.Instantiate(targetObject, containerObject, false);
        poolObject.name = targetObject.name;
        poolObject.gameObject.SetActive(false);
        objectPool.Enqueue(poolObject);

        return true;
    }

    public bool GetObject(out T Object)
    {
        Object = null;

        if (!containerObject)
            return false;

        if (0 == objectPool.Count)
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

    public void PutInPool(T Object)
    {
        Object.gameObject.SetActive(false);
        objectPool.Enqueue(Object);
    }

    public void ReturnBackPool()
    {
        if(containerObject)
        {
            foreach(Transform child in containerObject)
            {
                if(child.gameObject.activeSelf)
                {
                    if(child.TryGetComponent(out T Object))
                        PutInPool(Object);
                }
            }
        }
    }
}
