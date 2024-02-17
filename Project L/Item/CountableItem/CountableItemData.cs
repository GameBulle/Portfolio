using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountableItemData : ItemData
{
    [Header("Max Amount")]
    [SerializeField] int maxAmount;

    public int MaxAmount => maxAmount;
}
