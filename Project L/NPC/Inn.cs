using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : MonoBehaviour, IInteractionable
{
    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }

    private void Awake()
    {
        InteractionName = "¿©°ü";
        InteractionIcon = Resources.Load<Sprite>("Icon/Inn");
    }

    public void AddInteractionToList()
    {
        if(GameMgr.Instance.OpenUICount == 0)
            InterfaceMgr.Instance.AddInteraction(this.gameObject);
    }

    public void Interaction(Player player)
    {
        InterfaceMgr.Instance.SaveUI();
        RemoveInteractionToList();
    }

    public void InteractionGetItem(Player player)
    {

    }

    public void RemoveInteractionToList()
    {
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
    }
}
