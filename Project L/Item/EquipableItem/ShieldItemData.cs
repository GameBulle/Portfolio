using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Item", menuName = "ScriptableObject/Item/Shield", order = 7)]
public class ShieldItemData : EquipableItemData
{
    public override bool Use(Player player)
    {
        return player.EquipItem(this);
    }
}
