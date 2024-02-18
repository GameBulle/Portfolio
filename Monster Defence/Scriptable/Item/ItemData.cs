using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string itemName;
    [SerializeField] int maxCount;

    [Header("Material")]
    [SerializeField] int bone = 0;
    [SerializeField] int iron = 0;
    [SerializeField] int darkMaterial = 0;

    public int ID => id;
    public string ItemName => itemName;
    public int MaxCount => maxCount;
    public int Bone => bone;
    public int Iron => iron;
    public int DarkMaterial => darkMaterial;
}
