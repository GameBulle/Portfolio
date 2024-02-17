using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour, IInteractionable
{
    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }

    [SerializeField] NPCData npcData;

    private void Awake()
    {
        InteractionName = "º¸±â";
        InteractionIcon = Resources.Load<Sprite>("Icon/Pointer");
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
