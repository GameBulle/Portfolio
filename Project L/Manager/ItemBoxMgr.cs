using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemBoxMgr : Spawner<ItemBox>
{
    [SerializeField] TreasureBoxData[] boxes;

    static ItemBoxMgr instance = null;
    public static ItemBoxMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<ItemBoxMgr>();
                if(!instance)
                    instance = new GameObject("TreasureBoxManager").AddComponent<ItemBoxMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public enum TreasureBoxState { Close, Opened}

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    public void Initialize()
    {
        Array.Sort(boxes, (a, b) =>
        {
            if (a.MapID == b.MapID)
                return ((a.BoxID < b.BoxID) ? -1 : 1);
            else
                return ((a.MapID < b.MapID) ? -1 : 1);
        });
        InitializeSpawner();
        SetTreasureBoxes(0);
    }

    public void SetTreasureBoxes(int mapID)
    {
        for(int i=0;i<boxes.Length; i++)
        {
            if (mapID == boxes[i].MapID)
            {
                ItemBox box = Spawn(0, boxes[i].BoxPos, boxes[i].BoxAngle);
                box.gameObject.SetActive(true);
                box.InitTreasureItemBox(boxes[i]);
                box.OnDestroyEvent += (box) => GiveBackItem(box);
            }
        }
    }

    public void SetDropBoxes(Vector3 pos, int dropTableKey, string monsterName)
    {
        ItemBox box = Spawn(1, pos);
        box.gameObject.SetActive(true);
        box.InitDropItemBox(dropTableKey, monsterName);
        box.OnDestroyEvent += (box) => GiveBackItem(box);
    }

    public void AllRetunPool()
    {
        ReturnPool();
    }
}
