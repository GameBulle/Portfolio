using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Item", menuName = "ScriptableObject/Item/Sword", order = 8)]
public class SwordItemData : EquipableItemData
{
    public override bool Use(Player player)
    {
        return player.EquipItem(this);
    }
}
