using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractionable
{
    [SerializeField] PortalData data;

    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }
    bool enterPortal;

    private void Awake()
    {
        StringBuilder sb = new();
        sb.Append(MapMgr.Instance.GetMapData(data.NextMapID).MapName);
        sb.Append(" Æ÷Å»");
        InteractionName = sb.ToString();
        InteractionIcon = Resources.Load<Sprite>("Icon/Portal");
        enterPortal = false;
    }

    public void Interaction(Player player)
    {
        SoundMgr.Instance.PlaySFXAudio("Portal");
        SceneLoader.Instance.LoadScene(data);
        enterPortal = true;
        RemoveInteractionToList();
    }

    public void InteractionGetItem(Player player)
    {

    }

    public void AddInteractionToList()
    {
        if(!enterPortal)
            InterfaceMgr.Instance.AddInteraction(this.gameObject);
    }

    public void RemoveInteractionToList()
    {
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
    }
}
