using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class BulletFactory : ObjectFactory<Bullet>
{
    public override void Initialize(Transform parent)
    {
        Array.Sort(prefabs, (a, b) => (a.ID < b.ID ? -1 : 1));
        StringBuilder sb = new();
        parent_container = parent;
        object_container = new Transform[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            sb.Append(prefabs[i].name);
            object_container[i] = new GameObject(sb.ToString()).transform;
            object_container[i].SetParent(parent_container);
            sb.Clear();
        }
    }

    public override Bullet CreateGameObject(int id)
    {
        Bullet bullet = MonoBehaviour.Instantiate(prefabs[id], object_container[id]);
        return bullet;
    }


}
