using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractionable
{
    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }

    [SerializeField] ItemData[] items;
    [SerializeField] SpriteRenderer minimapIcon;
    [SerializeField] string shopName;
    Player player;

    private void Awake()
    {
        InteractionName = shopName;
        InteractionIcon = Resources.Load<Sprite>("Icon/Shop");
        minimapIcon.transform.eulerAngles = new Vector3(90f, 180f - transform.eulerAngles.z, 0f);
        Array.Sort(items, (a, b) => (a.ID < b.ID) ? -1 : 1);
    }

    public void Interaction(Player player)
    {
        this.player = player;
        player.Stop();
        InterfaceMgr.Instance.InitializeShop(this, items);
        InterfaceMgr.Instance.InventoryUI();
        GameMgr.Instance.isShop = true;
        RemoveInteractionToList();
    }

    public void InteractionGetItem(Player plyaer)
    {

    }

    public void AddInteractionToList()
    {
        if (!GameMgr.Instance.isShop)
            InterfaceMgr.Instance.AddInteraction(this.gameObject);
    }

    public void RemoveInteractionToList()
    {
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
    }

    public bool PurchaseItem(int itemID)
    {
        if (player.MinusGold(ItemMgr.Instance.GetItem(itemID).BuyPrice))
        {
            DropItem di = new();
            di.id = itemID;
            di.amount = 1;
            player.GetItem(di);
            SoundMgr.Instance.PlaySFXAudio("Buy");
            return true;
        }
        return false;
    }

    public void CancelSellItem(int itemID)
    {
        DropItem di = new();
        di.id = itemID;
        di.amount = 1;
        player.GetItem(di);
    }
    public string GetShopName() { return shopName; }
    public void SellItem(int gold) { player.PlusGold(gold); }
}
