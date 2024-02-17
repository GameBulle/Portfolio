using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Amulet Item", menuName = "ScriptableObject/Item/Amulet", order = 4)]
public class AmuletItemData : EquipableItemData
{
    public override bool Use(Player player)
    {
        return player.EquipItem(this);
    }
}
