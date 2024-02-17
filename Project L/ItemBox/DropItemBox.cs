using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemBox : ItemBox
{
    protected override void AfterGet()
    {
        if (dropItem.Count == 0)
        {
            InterfaceMgr.Instance.InteractionUIExit();
            RemoveInteractionToList();
            gameObject.SetActive(false);
            InterfaceMgr.Instance.DropItemBoxUIExit();
        }
    }
}
