using System;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopItemSlot shopItemSlotPrefab;
    [SerializeField] Vector2 margin;
    [SerializeField] RectTransform[] contentAreas;
    [SerializeField] GameObject purchasePage;
    [SerializeField] GameObject sellPage;
    [SerializeField] GameObject rePurchasePage;
    [SerializeField] RectTransform sellBottomArea;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] RectTransform Bg;
    [SerializeField] TextMeshProUGUI shopName;

    [Header("PopUp UI")]
    [SerializeField] ShopPopUpUI popUpUI;
    [SerializeField] GameObject lackOfGoldUI;

    enum ShopPage { Purchase,Sell,RePurchase}

    Shop shop;
    ShopItemSlot[] purchaseItemSlots;
    ShopItemSlot[] sellItemSlots;
    ShopItemSlot[] rePurchaseItemSlots;

    float sellBottomAreaHeight;
    float BgHeight;
    int sellGold;
    int purchaseItemID;
    int rePurchaseItemIndex;

    public void Initialize(Shop shop, ItemData[] items)
    {
        gameObject.SetActive(true);
        //int size = items.Length;
        int size = 18;
        this.shop = shop;
        rePurchaseItemIndex = -1;
        sellBottomAreaHeight = sellBottomArea.sizeDelta.y;
        BgHeight = Bg.sizeDelta.y;
        goldText.text = "0";
        shopName.text = shop.GetShopName();
        purchaseItemSlots = new ShopItemSlot[size];
        sellItemSlots = new ShopItemSlot[size];
        rePurchaseItemSlots = new ShopItemSlot[size];

        MakeSlots(size, ShopPage.Purchase, items);
        MakeSlots(size, ShopPage.Sell);
        MakeSlots(size, ShopPage.RePurchase);

        purchasePage.SetActive(true);
        sellPage.SetActive(false);
        rePurchasePage.SetActive(false);
        popUpUI.gameObject.SetActive(false);
        lackOfGoldUI.gameObject.SetActive(false);
    }

    void MakeSlots(int size, ShopPage page, ItemData[] items = null )
    {
        Vector2 startPos = new Vector2(margin.x, -margin.y);
        Vector2 curPos = startPos;

        for (int i = 0; i < size; i++)
        {
            ShopItemSlot slot = Instantiate(shopItemSlotPrefab);
            RectTransform rt = slot.GetComponent<RectTransform>();

            rt.SetParent(contentAreas[(int)page]);

            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = curPos;
            rt.gameObject.SetActive(true);
            rt.gameObject.name = $"Shop Item Slot[{i}]";

            ShopItemSlot shopSlot = rt.GetComponent<ShopItemSlot>();
            shopSlot.Index = i;
            
            switch(page)
            {
                case ShopPage.Purchase:
                    if(i<items.Length)
                    {
                        shopSlot.Initailize(MakeSlot.SlotType.Shop_Purchase, items[i]);
                        purchaseItemSlots[i] = shopSlot;
                    }
                    else
                        shopSlot.Initailize(MakeSlot.SlotType.Shop_Purchase);
                    break;
                case ShopPage.Sell:
                    shopSlot.Initailize(MakeSlot.SlotType.Shop_Sell);
                    sellItemSlots[i] = shopSlot;
                    break;
                case ShopPage.RePurchase:
                    shopSlot.Initailize(MakeSlot.SlotType.Shop_RePurchase);
                    rePurchaseItemSlots[i] = shopSlot;
                    break;
            }

            if (((i + 1) % 3 == 0 && i > 0))
            {
                curPos.y -= rt.sizeDelta.y + margin.y;
                curPos.x = startPos.x;
            }
            else
                curPos.x += rt.sizeDelta.x + margin.x;
        }
    }

    public void PurchaseItemCheck(int itemID)
    {
        purchaseItemID = itemID;
        popUpUI.PopUpActivate(itemID);
    }

    public void PurchaseItem()
    {
        popUpUI.gameObject.SetActive(false);
        if (!shop.PurchaseItem(purchaseItemID))
            lackOfGoldUI.gameObject.SetActive(true);
        else if(rePurchaseItemIndex != -1)
        {
            rePurchaseItemSlots[rePurchaseItemIndex].ClearSlot();
            rePurchaseItemIndex = -1;
            UpdateRePurchaseItemList();
        }
      
    }
    
    public void DisabledLackOfGoldUI()
    {
        lackOfGoldUI.SetActive(false);
    }

    public void SellCancel(int itemID, int index)
    {
        shop.CancelSellItem(itemID);
        sellGold -= ItemMgr.Instance.GetItem(itemID).SellPrice;
        goldText.text = sellGold.ToString();
        sellItemSlots[index].ClearSlot();
        UpdateSellItemList();
    }

    public void RePurchaseItemCheck(int itemID, int index)
    {
        purchaseItemID = itemID;
        rePurchaseItemIndex = index;
        popUpUI.PopUpActivate(itemID);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameMgr.Instance.isShop = false;
    }

    public void PurchasePage()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        purchasePage.SetActive(true);
        sellPage.SetActive(false);
        rePurchasePage.SetActive(false);
        Bg.sizeDelta = new Vector2(Bg.sizeDelta.x, BgHeight);
    }

    public void SellPage()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        sellPage.SetActive(true);
        purchasePage.SetActive(false);
        rePurchasePage.SetActive(false);
        Bg.sizeDelta = new Vector2(Bg.sizeDelta.x, BgHeight + sellBottomAreaHeight);
    }

    public void RePurchasePage()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        rePurchasePage.SetActive(true);
        purchasePage.SetActive(false);
        sellPage.SetActive(false);
        Bg.sizeDelta = new Vector2(Bg.sizeDelta.x, BgHeight);
    }

    public void AddSellItemToArray(int itemID)
    {
        for(int i=0;i<sellItemSlots.Length;i++)
        {
            if (!sellItemSlots[i].HasItem)
            {
                sellItemSlots[i].AddItem(itemID, true);
                sellGold += ItemMgr.Instance.GetItem(itemID).SellPrice;
                UpdateSellGold();
                return;
            }
        }
    }

    public void AddRePurchaseItemToArray(int itemID)
    {
        for(int i=0;i<rePurchaseItemSlots.Length;i++)
        {
            if (!rePurchaseItemSlots[i].HasItem)
            {
                rePurchaseItemSlots[i].AddItem(itemID, false);
                return;
            }    
        }
    }

    void UpdateSellGold() {  goldText.text = sellGold.ToString();}

    public void UpdateSellItemList()
    {
        for (int i = 0; i < sellItemSlots.Length - 1; i++)
        {
            if (!sellItemSlots[i].HasItem)
            {
                for(int j = i+1;j<sellItemSlots.Length;j++)
                {
                    if (sellItemSlots[j].HasItem)
                    {
                        sellItemSlots[i].AddItem(sellItemSlots[j].GetItemID(), true);
                        sellItemSlots[j].ClearSlot();
                        break;
                    }
                }
            }
        }
    }

    public void UpdateRePurchaseItemList()
    {
        for(int i=0;i<rePurchaseItemSlots.Length-1;i++)
        {
            if(!rePurchaseItemSlots[i].HasItem)
            {
                for (int j = i + 1; j < rePurchaseItemSlots.Length; j++) 
                {
                    if(rePurchaseItemSlots[j].HasItem)
                    {
                        rePurchaseItemSlots[i].AddItem(rePurchaseItemSlots[j].GetItemID(), false);
                        rePurchaseItemSlots[j].ClearSlot();
                        break;
                    }
                }
            }
        }
    }

    public void ClickSell()
    {
        SoundMgr.Instance.PlaySFXAudio("Sell");
        for(int i=0;i<sellItemSlots.Length;i++)
        {
            if (sellItemSlots[i].HasItem)
            {
                rePurchaseItemSlots[i].AddItem(sellItemSlots[i].GetItemID(), false);
                sellItemSlots[i].ClearSlot();
            }
        }
        shop.SellItem(sellGold);
        sellGold = 0;
        goldText.text = sellGold.ToString();
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if(GameMgr.Instance.OpenUICount == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
