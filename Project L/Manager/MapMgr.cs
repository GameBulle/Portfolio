using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMgr : MonoBehaviour
{
    static MapMgr instance = null;

    public static MapMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<MapMgr>();
                if (!instance)
                    instance = new GameObject("MapManager").AddComponent<MapMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    [SerializeField] MapData[] mapDatas;
    [SerializeField] float ratio;
    Vector2 benchmarkPos;
    Vector2 benchmark2Pos;
    Vector2 playerPos;

    public void Initialize()
    {
        Array.Sort(mapDatas, (a, b) => (a.MapID < b.MapID) ? -1 : 1);
        InterfaceMgr.Instance.MapInit(mapDatas);
    }

    public MapData GetMapData(int mapID) {  return mapDatas[mapID]; }
    public MapData GetMapData(string mapName)
    {
        return (Array.Find(mapDatas, (a) => a.MapName == mapName));
    }

}
