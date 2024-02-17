using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseData:ScriptableObject
{
    [SerializeField] protected int id = 0;
    [SerializeField] protected string monsterName;
    [SerializeField] protected LayerMask targetLayer;

    public int ID => id;
    public string MonsterName => monsterName;
    public LayerMask TargetLayer => targetLayer;
}
