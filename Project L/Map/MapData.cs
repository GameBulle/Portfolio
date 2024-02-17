using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct SpawnMonsterData
{
    public int spawnMonsterID;
    public Vector3 spawnPos;
    public Vector3 spawnAngle;
}

[CreateAssetMenu(fileName = "Map Data", menuName = "ScriptableObject/Map/Map", order = 0)]
public class MapData : ScriptableObject
{
    [SerializeField] int mapID;
    [SerializeField] string mapName;
    [SerializeField] Sprite mapImage;
    [SerializeField] SpawnMonsterData[] spawnMonsterData;
    [SerializeField] bool isRotate;

    public int MapID => mapID;
    public string MapName => mapName;
    public Sprite MapImange => mapImage;
    public SpawnMonsterData[] SpawnMonsterData => spawnMonsterData;
    public bool IsRotate => isRotate;
}
