using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("ID")]
    [SerializeField] int id;

    [Header("Name")]
    [SerializeField] string itemName;

    [Header("Tooltip")]
    [SerializeField] string toolTip;

    [Header("Icon")]
    [SerializeField] Sprite icon;

    [Header("Item Type")]
    [SerializeField] ItemType type;

    [Header("Price")]
    [SerializeField] int sellPrice;
    [SerializeField] int buyPrice;

    public enum ItemType { Countable,Equipable}

    public int ID => id;
    public string ItemName => itemName;
    public string ToolTip => toolTip;
    public Sprite Icon => icon;
    public ItemType Type => type;
    public int SellPrice => sellPrice;
    public int BuyPrice => buyPrice;

    public abstract bool Use(Player player);
}
