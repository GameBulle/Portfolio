using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : Spawner<Arrow>
{
    static ArrowSpawner instance = null;

    public static ArrowSpawner Instance
    {
        get
        {
            if(null == instance)
            {
                instance = FindObjectOfType<ArrowSpawner>();
                if (!instance)
                    instance = new GameObject("ArrowObjectMgr").AddComponent<ArrowSpawner>();

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
        InitializeSpawner();
    }

    public void SpawnArrow(Vector2 dir, Vector2 pos, float damage, float penetration, float speed)
    {
        Arrow arrow = Spawn(0, pos);
        arrow.OnShot(dir, damage, penetration, speed);
    }

    public void WaveClear()
    {
        ReturnPool();
    }

    public void GameOver()
    {
        WaveClear();
        gameObject.SetActive(false);
    }
}