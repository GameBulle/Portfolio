using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionObject : MonoBehaviour, IInteractionable
{
    [SerializeField] string ActionName;
    [SerializeField] public float ActionTime;
    [SerializeField] public string ActionText;
    [SerializeField] protected int questID;

    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }

    private void Awake()
    {
        InteractionName = ActionName;
        InteractionIcon = Resources.Load<Sprite>("Icon/Loot");
    }

    public void AddInteractionToList()
    {
        if(questID == -1)
        {
            InterfaceMgr.Instance.AddInteraction(this.gameObject);
            return;
        }
            
        if (QuestMgr.Instance.GetQuestData(questID).State == QuestMgr.QuestState.Proceed && !GameMgr.Instance.IsAction)
            InterfaceMgr.Instance.AddInteraction(this.gameObject);
    }

    public void Interaction(Player player)
    {
        SoundMgr.Instance.PlaySFXAudio("UI_Open");
        InterfaceMgr.Instance.ActionInfo(this);
        GameMgr.Instance.IsAction = true;
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
    }

    public void InteractionGetItem(Player player)
    {

    }

    public void RemoveInteractionToList()
    {
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
        if (GameMgr.Instance.IsAction)
            InterfaceMgr.Instance.CancelAction();
    }

    public virtual void Action() { }
}
