using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractionable
{
    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }

    [SerializeField] SpriteRenderer minimapIcon;
    [SerializeField] protected NPCData npcData;

    protected virtual void Awake()
    {
        InteractionName = "¸»ÇÏ±â";
        InteractionIcon = Resources.Load<Sprite>("Icon/NPC_talk");
    }

    public void Interaction(Player player)
    {
        InterfaceMgr.Instance.TalkInfo(npcData);
        RemoveInteractionToList();
        
    }

    public void InteractionGetItem(Player plyaer)
    {
        
    }

    public void AddInteractionToList()
    {
        if (!GameMgr.Instance.IsTalk)
            InterfaceMgr.Instance.AddInteraction(this.gameObject);
    }

    public void RemoveInteractionToList()
    {
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
    }
}
