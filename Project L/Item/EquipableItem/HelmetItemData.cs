using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Helmet Item", menuName = "ScriptableObject/Item/Helmet", order = 5)]
public class HelmetItemData : EquipableItemData
{
    public override bool Use(Player player)
    {
        return player.EquipItem(this);
    }
}
