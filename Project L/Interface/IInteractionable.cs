using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionable
{
    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }

    public void Interaction(Player player);
    public void InteractionGetItem(Player player);

    public void AddInteractionToList();
    public void RemoveInteractionToList();
}
