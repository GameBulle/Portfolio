using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stone Item", menuName = "ScriptableObject/Item/Stone", order = 0)]
public class StoneItemData : CountableItemData
{
    [SerializeField] RecallStone stonePrefab;
    public override bool Use(Player player)
    {
        RecallStone stone = Instantiate(stonePrefab);
        stone.SetPosition(player.transform.position);
        stone.gameObject.SetActive(true);
        SoundMgr.Instance.PlaySFXAudio("Stone");
        return true;
    }
}
