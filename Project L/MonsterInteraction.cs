using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInteraction : MonoBehaviour ,IInteractionable
{
    [SerializeField] Monster monster;
    [SerializeField] bool isExecuted = true;

    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }

    private void Awake()
    {
        InteractionName = "Ã³Çü";
        InteractionIcon = Resources.Load<Sprite>("Icon/execute");
    }

    public void Interaction(Player player)
    {
        monster.SetExecuted(player.ExecutePos);
        player.OnExecuteState();
        monster.OnExecutedState();
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.enabled = false;
        RemoveInteractionToList();
    }

    public void InteractionGetItem(Player player)
    {

    }

    public void AddInteractionToList()
    {
        if (isExecuted)
        {
            if (!monster.IsThereTarget && !monster.isDead)
                InterfaceMgr.Instance.AddInteraction(this.gameObject);
            else
                RemoveInteractionToList();
        }
    }

    public void RemoveInteractionToList()
    {
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
    }
}
