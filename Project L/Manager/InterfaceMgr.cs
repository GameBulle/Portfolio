using System.Collections;
using UnityEngine;

public class InterfaceMgr : MonoBehaviour
{
    [Header("In Game UI")]
    [SerializeField] PlayerUI playerUI;
    [SerializeField] Draggable draggable;
    [SerializeField] ShopUI shopUI;
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] SaveUI saveUI;
    [SerializeField] TextUI textUI;
    [SerializeField] OptionUI optionUI;

    static InterfaceMgr instance = null;
    public static InterfaceMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InterfaceMgr>();
                if (!instance)
                    instance = new GameObject("InterfaceManager").AddComponent<InterfaceMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    private void Start()
    {
        gameObject.SetActive(true);
        textUI.gameObject.SetActive(false);
        mainMenuUI.Initialize();
        SoundMgr.Instance.PlayBackgroundAudio("Main");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainTile()
    {
        gameObject.SetActive(true);
        mainMenuUI.Initialize();
        draggable.gameObject.SetActive(false);
        playerUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Initailize(Player player)
    {
        playerUI.Initailize(player);
        draggable.Initialize();
        saveUI.Initialize(player);
        shopUI.gameObject.SetActive(false);
    }

    public void LevelUP(float maxHP)
    {
        playerUI.LevelUP(maxHP);
    }

    public void GuardGaugeUpdate(float gauge)
    {
        playerUI.GuardGaugeUpdate(gauge);
    }

    public void GuardGaugeOnOff(bool isOn)
    {
        playerUI.GuardGaugeOnOff(isOn);
    }

    public void LockOnImageOn()
    {
        playerUI.LockOnImageOn();
    }

    public void LockOnImagePosUpdate(Vector3 pos)
    {
        playerUI.LockOnImagePosUpdate(pos);
    }

    public void LockOnImageOff()
    {
        playerUI.LockOnImageOff();
    }

    public void UpdateHP(float hp)
    {
        playerUI.UpdateHP(hp);
    }

    public void UpdateMP(float mp)
    {
        playerUI.UpdateMP(mp);
    }

    public void UpdateSP(float sp)
    {
        playerUI.UpdateSP(sp);
    }

    public void InventoryUI()
    {
        draggable.InventoryUI();
    }

    public void InteractionUIActive()
    {
        playerUI.InteractionUIActive();
    }

    public void InteractionUIExit()
    {
        playerUI.InteractionUIExit();
    }

    public void DropItemBoxUIExit()
    {
        playerUI.DropItemBoxUIExit();
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void LinkDropItemBox(ItemBox box)
    {
        playerUI.LinkDropItemBox(box);
    }

    public int GetInventorySize()
    {
        return draggable.GetInventorySize();
    }

    public int GetQuickSlotSize() { return draggable.GetQuickSlotSize(); }
    public int GetSkillSlotSize() { return draggable.GetSkillSlotSize(); }

    public void AddItemToInventoryUI(DropItem item,int index)
    {
        draggable.AddItemInventoryUI(item,index);
    }

    public void AddItemToQuickSlotUI(DropItem item,int slotIndex)
    {
        draggable.AddItemQuickSlotUI(item,slotIndex);
    }

    public void AddItemToPlayerInfoUI(DropItem item, int index)
    {
        playerUI.AddItemPlayerInfoUI(item, index);
    }

    public void GetUseItemSlotIndex(int index)
    {
        draggable.GetUseItemSlotIndex(index);
    }

    public void ClearSlot(MakeSlot.SlotType slotType, int index)
    {
        draggable.ClearSlot(slotType,index);
    }

    public void SetInventory(Inventory inventory)
    {
        draggable.SetInventory(inventory);
    }

    public void SetQuickSlot(QuickSlot quickSlot) { draggable.SetQuickSlot(quickSlot); }
    public void SetSkillSlot(SkillSlot skillSlot) { draggable.SetSkillSlot(skillSlot); }
    
    public void SetDragStartIndex(MakeSlot.SlotType slotType, int slotIndex)
    {
        draggable.SetDragStartInfo(slotType, slotIndex);
    }

    public void SetDragEndInfo(MakeSlot.SlotType slotType, int slotIndex, bool hasItem)
    {
        draggable.SetDragEndInfo(slotType, slotIndex, hasItem);
    }

    public void DragItem()
    {
        draggable.DragItem();
    }

    public void DragSkill()
    {
        draggable.DragSkill();
    }

    public void InitDragInfo()
    {
        draggable.InitDragInfo();
    }

    public void InputUIActive(Vector3 mousePos, int itemAmount, int slotIndex, MakeSlot.SlotType slotType)
    {
        playerUI.InputUIActive(mousePos, itemAmount, slotIndex, slotType);
    }

    public bool CheckQuickSlotItem(int index) { return draggable.CheckQuickSlotItem(index);}

    public void InputAmount(MakeSlot.SlotType slotType, int slotIndex, int amount)
    {
        draggable.InputAmount(slotType, slotIndex, amount);
    }

    public void AddInteraction(GameObject gameObject)
    {
        playerUI.AddInteraction(gameObject);
    }

    public void RemoveInteraction(GameObject gameObject)
    {
        if(playerUI)
            playerUI.RemoveInteraction(gameObject);
    }

    public void UnequipItem(int index)
    {
        playerUI.UnequipItem(index);
    }

    public void PlayerInfoClearSlot(int index)
    {
        playerUI.PlayerInfoClearSlot(index);
    }

    public void SetPlayerInfo(Player player)
    {
        playerUI.SetPlayerInfo(player);
    }

    public void SetSkill(int id, int index) { draggable.SetSkill(id, index); }

    public void PlayerInfoUI()
    {
        playerUI.PlayerInfoUI();
    }

    public void UpdatePlayerStatusInfo(Status level, Status equipment)
    {
        playerUI.UpdatePlayerStatusInfo(level, equipment);
    }

    public void UpdatePlayerLevelInfo(int level, float EXP, float nextLevelEXP)
    {
        playerUI.UpdatePlayerLevelInfo(level, EXP, nextLevelEXP);
    }

    public void SelectInteraction()
    {
        playerUI.SelectInteraction();
    }

    public void WheelUp()
    {
        playerUI.WheelUp();
    }

    public void WheelDown()
    {
        playerUI.WheelDown();
    }

    public void SkillUI()
    {
        playerUI.SkillUI();
    }

    public void InitSkills()
    {
        draggable.InitSkills();
    }

    public void MapUI(Transform playerTr)
    {
        playerUI.MapUI(playerTr);
    }

    public void ESCUI()
    {
        playerUI.ESCUI();
    }

    public void MapInit(MapData[] mapDatas)
    {
        playerUI.MapInit(mapDatas);
    }

    public void ChangeCurrMap(int currMapID)
    {
        playerUI.ChangeCurrMap(currMapID);
    }

    public void TalkInfo(NPCData npcData)
    {
        playerUI.TalkInfo(npcData);
    }

    public void BossUIActivate(string name, float maxHp, float gauge)
    {
        playerUI.BossUIActivate(name, maxHp, gauge);
    }

    public void BossUIDisabled()
    {
        playerUI.BossUIDisabled();
    }

    public void UpdateBossHP(float hp)
    {
        playerUI.UpdateBossHP(hp);
    }

    public void UpdateBossGrogyGauge(float gauge)
    {
        playerUI.UpdateBossGrogyGauge(gauge);
    }

    public void InitializeShop(Shop shop, ItemData[] items)
    {
        shopUI.Initialize(shop,items);
    }

    public void AddSellItemToArray(int itemID, int index)
    {
        shopUI.AddSellItemToArray(itemID);
        draggable.SellItem(index);
    }

    public void PurchaseItemCheck(int itemID)
    {
        shopUI.PurchaseItemCheck(itemID);
    }

    public void RePurchaseItem(int itemID, int index)
    {
        shopUI.RePurchaseItemCheck(itemID, index);
    }

    public void SellCancel(int itemID, int index)
    {
        shopUI.SellCancel(itemID, index);
    }

    public void QuestWindow(int questID, bool detail = false)
    {
        playerUI.QuestWindow(questID, detail);
    }

    public void AddQuest(int questID)
    {
        playerUI.AddQuest(questID);
    }

    public void ActionInfo(ActionObject action)
    {
        playerUI.ActionInfo(action);
    }

    public void CancelAction()
    {
        playerUI.CancelAction();
    }

    public void SaveUI()
    {
        if (saveUI.gameObject.activeSelf == true)
            saveUI.gameObject.SetActive(false);
        else
            saveUI.gameObject.SetActive(true);
    }

    public void GameStart()
    {
        mainMenuUI.gameObject.SetActive(false);
    }

    public void OptionUI()
    {
        if(optionUI.gameObject.activeSelf == true)
            optionUI.gameObject.SetActive(false);
        else
            optionUI.gameObject.SetActive(true);
    }

    public void SetTextUI(string text, float time)
    {
        textUI.SetTextUI(text, time);
    }

    public void SetLevelUpUI(int level)
    {
        playerUI.SetLevelUpUI(level);
    }

    public void UpdateGold(int gold) { draggable.UpdateGold(gold); }
}
