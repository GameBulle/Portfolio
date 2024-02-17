using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Item", menuName = "ScriptableObject/Item/Armor", order = 3)]
public class ArmorItemData : EquipableItemData
{
    public override bool Use(Player player)
    {
        return player.EquipItem(this);
    }
}
