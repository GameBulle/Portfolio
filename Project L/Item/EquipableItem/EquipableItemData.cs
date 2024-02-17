using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipableItemData : ItemData
{
    [Header("Equip Type")]
    [SerializeField] protected ItemMgr.EquipType equipType;

    [Header("Item Status")]
    [SerializeField] protected Status status;

    public ItemMgr.EquipType EquipType => equipType;
    public Status Status => status;
}
