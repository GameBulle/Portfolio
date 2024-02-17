using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureBox Data", menuName = "ScriptableObject/TreasureBox/TreasureBox Data", order = 0)]
public class TreasureBoxData : ScriptableObject
{
    [SerializeField] int mapID;
    [SerializeField] int boxID;
    [SerializeField] Vector3 boxPos;
    [SerializeField] Vector3 boxAngle;
    [SerializeField] DropItem[] items;
    [SerializeField] int gold;

    public ItemBoxMgr.TreasureBoxState State { get; set; }

    private void Awake()
    {
        State = ItemBoxMgr.TreasureBoxState.Close;
    }

    public int MapID => mapID;
    public int BoxID => boxID;
    public Vector3 BoxPos => boxPos;
    public Vector3 BoxAngle => boxAngle;
    public DropItem[] Items => items;
    public int Gold => gold;
}
