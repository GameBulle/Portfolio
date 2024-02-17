using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portion Item", menuName = "ScriptableObject/Item/Portion", order = 2)]
public class PortionItemData : CountableItemData
{
    [Header("Type")]
    [SerializeField] ItemMgr.PortionType portion;

    [Header("Value")]
    [SerializeField] float value;

    public override bool Use(Player player)
    {
         return player.RestoreHealth(portion, value);
    }
}
