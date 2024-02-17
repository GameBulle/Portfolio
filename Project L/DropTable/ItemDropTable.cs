using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DropItemData
{
    public int iD;
    public float percent;
    [Header("Min : x, Max : y")]
    public Vector2Int amount;
}

[CreateAssetMenu(fileName = "DropTable Data", menuName = "ScriptableObject/DropTable Data", order = 1)]
public class ItemDropTable : ScriptableObject
{
    [Header("Drop Table Key")]
    [SerializeField] int id;

    [Header("Drop Items")]
    [SerializeField]  DropItemData[] dropDatas;

    [Header("Gold Min : x, Max : y")]
    public Vector2Int gold;

    public int ID => id;
    public DropItemData[] DropDatas => dropDatas;
}
