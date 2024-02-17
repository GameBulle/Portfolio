using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Guard Gauge Slider")]
    [SerializeField] Slider guardGauge;

    [Header("HP Slider")]
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;

    [Header("MP Slider")]
    [SerializeField] Slider mpSlider;
    [SerializeField] TextMeshProUGUI mpText;

    [Header("SP Slider")]
    [SerializeField] Slider spSlider;
    [SerializeField] TextMeshProUGUI spText;

    [SerializeField] GameObject lockOnImage;
    [SerializeField] GameObject interactionUI;
    [SerializeField] ItemBoxUI dropItemBoxUI;
    [SerializeField] InputUI inputUI;
    [SerializeField] InteractionListUI interactionListUI;
    [SerializeField] PlayerInfoUI playerInfoUI;
    [SerializeField] SkillUI skillUI;
    [SerializeField] MapUI mapUI;
    [SerializeField] TalkUI talkUI;
    [SerializeField] BossUI bossUI;
    [SerializeField] QuestWindowUI questWindow;
    [SerializeField] QuestListUI questList;
    [SerializeField] ActionUI actionUI;
    [SerializeField] MinimapUI minimapUI;
    [SerializeField] ESCUI escUI;
    [SerializeField] LevelUpUI levelUpUI;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = lockOnImage.GetComponent<RectTransform>();
    }

    public void Initailize(Player player)
    {
        gameObject.SetActive(true);

        guardGauge.maxValue = player.GetPlayerData().GP;
        guardGauge.value = guardGauge.maxValue;

        hpSlider.maxValue = player.GetPlayerData().HP;
        hpSlider.value = hpSlider.maxValue;
        UpdateHpText();

        mpSlider.maxValue = player.GetPlayerData().MP;
        mpSlider.value = mpSlider.maxValue;
        UpdateMpText();

        spSlider.maxValue = player.GetPlayerData().SP;
        spSlider.value = spSlider.maxValue;
        UpdateSpText();

        dropItemBoxUI.Initialize(MakeSlot.SlotType.DropItemBox);
        interactionListUI.Initialize(player);

        playerInfoUI.Initialize(MakeSlot.SlotType.Equipment, player.GetPlayerData());

        skillUI.Initialize(player);

        questList.Initialize();

        guardGauge.gameObject.SetActive(false);
        lockOnImage.gameObject.SetActive(false);
        interactionUI.gameObject.SetActive(false);
        dropItemBoxUI.gameObject.SetActive(false);
        inputUI.gameObject.SetActive(false);
        talkUI.gameObject.SetActive(false);
        bossUI.gameObject.SetActive(false);
        questWindow.gameObject.SetActive(false);
        actionUI.gameObject.SetActive(false);
        escUI.gameObject.SetActive(false);
        levelUpUI.gameObject.SetActive(false);
    }

    public void LevelUP(float maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = maxHP;
        UpdateHpText();
        playerInfoUI.LevelUP(maxHP);
        skillUI.PlayerLevelUP();
    }

    void UpdateHpText()
    {
        StringBuilder sb = new();
        sb.Append((int)hpSlider.value);
        sb.Append(" / ");
        sb.Append(hpSlider.maxValue);
        hpText.text = sb.ToString();
    }

    void UpdateMpText()
    {
        StringBuilder sb = new();
        sb.Append((int)mpSlider.value);
        sb.Append(" / ");
        sb.Append(mpSlider.maxValue);
        mpText.text = sb.ToString();
    }

    void UpdateSpText()
    {
        StringBuilder sb = new();
        sb.Append((int)spSlider.value);
        sb.Append(" / ");
        sb.Append((int)spSlider.maxValue);
        spText.text = sb.ToString();
    }

    public void GuardGaugeUpdate(float gauge)
    {
        guardGauge.value = gauge;
    }

    public void GuardGaugeOnOff(bool isOn)
    {
        if (isOn)
            guardGauge.gameObject.SetActive(true);
        else
            guardGauge.gameObject.SetActive(false);
    }

    public void LockOnImagePosUpdate(Vector3 pos)
    {
        rectTransform.position = GameMgr.Instance.mainCam.WorldToScreenPoint(pos + Vector3.up * 10);
    }

    public void LockOnImageOn()
    {
        lockOnImage.gameObject.SetActive(true);
    }

    public void LockOnImageOff()
    {
        lockOnImage.gameObject.SetActive(false);
    }

    public void UpdateHP(float hp)
    {
        hpSlider.value = hp;
        UpdateHpText();
        playerInfoUI.UpdateHP(hp);
    }

    public void UpdateMP(float mp)
    {
        mpSlider.value = mp;
        UpdateMpText();
        playerInfoUI.UpdateMP(mp);
    }

    public void UpdateSP(float sp)
    {
        spSlider.value = sp;
        UpdateSpText();
        playerInfoUI.UpdateSP(sp);
    }

    public void InteractionUIActive()
    {
        interactionUI.gameObject.SetActive(true);
    }

    public void InteractionUIExit()
    {
        interactionUI.gameObject.SetActive(false);
    }

    public void DropItemBoxUIExit()
    {
        dropItemBoxUI.ClearDropItemBox();
        dropItemBoxUI.gameObject.SetActive(false);
    }

    public void LinkDropItemBox(ItemBox box)
    {
        dropItemBoxUI.LinkDropItemBox(box);
    }

    public void InputUIActive(Vector3 mousePos, int itemAmount, int slotIndex, MakeSlot.SlotType slotType)
    {
        inputUI.Initialize(mousePos, itemAmount, slotIndex, slotType);
    }

    public void AddInteraction(GameObject gameObject)
    {
        interactionListUI.AddInteraction(gameObject);
    }

    public void RemoveInteraction(GameObject gameObject)
    {
        interactionListUI.RemoveInteraction(gameObject);
    }

    public void AddItemPlayerInfoUI(DropItem item, int index)
    {
        playerInfoUI.AddItemToPlayerInfoUI(item, index);
    }

    public void UnequipItem(int index)
    {
        playerInfoUI.UnequipItem(index);
    }

    public void PlayerInfoClearSlot(int index)
    {
        playerInfoUI.ClearSlot(index);
    }

    public void SetPlayerInfo(Player player)
    {
        playerInfoUI.SetPlayer(player);
    }

    public void PlayerInfoUI()
    {
        if(playerInfoUI.gameObject.activeSelf == true)
            playerInfoUI.gameObject.SetActive(false);
        else
            playerInfoUI.gameObject.SetActive(true);
    }

    public void UpdatePlayerStatusInfo(Status level, Status equipment)
    {
        playerInfoUI.UpdatePlayerStatusInfo(level, equipment);
    }

    public void UpdatePlayerLevelInfo(int level, float EXP, float nextLevelEXP)
    {
        playerInfoUI.UpdatePlayerLevelInfo(level, EXP, nextLevelEXP);
    }

    public void SelectInteraction()
    {
        interactionListUI.SelectInteraction();
    }

    public void WheelUp()
    {
        interactionListUI.WheelUp();
    }

    public void WheelDown()
    {
        interactionListUI.WheelDown();
    }

    public void SkillUI()
    {
        if(skillUI.gameObject.activeSelf == true)
            skillUI.gameObject.SetActive(false);
        else
            skillUI.gameObject.SetActive(true);
    }

    public int GetSkillPoint() { return skillUI.SkillPoint; }

    public void MapUI(Transform playerTr)
    {
        if(mapUI.gameObject.activeSelf == true)
        {
            Time.timeScale = 1.0f;
            mapUI.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            mapUI.OpenMapUI(playerTr);
        }
    }

    public void ESCUI()
    {
        if(escUI.gameObject.activeSelf == true)
        {
            Time.timeScale = 1.0f;
            escUI.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            escUI.gameObject.SetActive(true);
        }
    }

    public void MapInit(MapData[] mapDatas)
    {
        mapUI.Initialize(mapDatas);
    }

    public void ChangeCurrMap(int currMapID)
    {
        mapUI.ChangeCurrMap(currMapID);
        minimapUI.ChangeCurrMapName(currMapID);
    }

    public void TalkInfo(NPCData npcData)
    {
        talkUI.TalkInfo(npcData);
    }

    public void BossUIActivate(string name, float maxHp, float gauge)
    {
        bossUI.BossUIActivate(name, maxHp, gauge);
    }

    public void BossUIDisabled()
    {
        bossUI.gameObject.SetActive(false);
    }

    public void UpdateBossHP(float hp)
    {
        bossUI.UpdateHP(hp);
    }

    public void UpdateBossGrogyGauge(float gauge)
    {
        bossUI.UpdateGrogyGauge(gauge);
    }

    public void QuestWindow(int questID, bool detail = false)
    {
        questWindow.QuestWindow(questID, detail);
    }

    public void AddQuest(int questID)
    {
        questList.AddQuest(questID);
    }

    public void ActionInfo(ActionObject action)
    {
        actionUI.ActionInfo(action);
    }

    public void CancelAction()
    {
        actionUI.gameObject.SetActive(false);
        GameMgr.Instance.IsAction = false;
    }

    public void SetLevelUpUI(int level)
    {
        levelUpUI.SetLevelUpUI(level);
    }
}
