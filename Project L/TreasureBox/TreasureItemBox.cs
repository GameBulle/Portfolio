using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TreasureItemBox : ItemBox
{
    [SerializeField] GameObject open;
    [SerializeField] GameObject close;

    public override void InitTreasureItemBox(TreasureBoxData data)
    {
        base.InitTreasureItemBox(data);

        if(data.State == ItemBoxMgr.TreasureBoxState.Close)
        {
            open.gameObject.SetActive(false);
            close.gameObject.SetActive(true);
        }
        else
        {
            open.gameObject.SetActive(true);
            close.gameObject.SetActive(false);
        }
    }

    protected override void AfterGet()
    {
        open.gameObject.SetActive(true);
        close.gameObject.SetActive(false);

        if (dropItem.Count == 0)
        {
            InterfaceMgr.Instance.InteractionUIExit();
            RemoveInteractionToList();
            InterfaceMgr.Instance.DropItemBoxUIExit();
        }
    }
}
