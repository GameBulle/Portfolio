using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoUI : MakeSlot
{
    Player player;

    [Header("Level")]
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI exp;

    [Header("Status Text")]
    [SerializeField] TextMeshProUGUI hp;
    [SerializeField] TextMeshProUGUI mp;
    [SerializeField] TextMeshProUGUI sp;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI defence;
    [SerializeField] TextMeshProUGUI recoveryMP;
    [SerializeField] TextMeshProUGUI recoverySP;
    [SerializeField] TextMeshProUGUI guardGauge;

    float maxHP;
    float maxMP;
    float maxSP;

    public void SetPlayer(Player player) { this.player = player; }

    public void Initialize(SlotType type, PlayerData playerData)
    {
        MakeSlotInit(type, 1);
        maxHP = playerData.HP;
        maxMP = playerData.MP;
        maxSP = playerData.SP;

        gameObject.SetActive(false);
    }

    public void AddItemToPlayerInfoUI(DropItem item, int index)
    {
        slots[index].SetSlot(ItemMgr.Instance.GetItem(item.id), item.amount);
    }

    public void ClearSlot(int index)
    {
        slots[index].ClearSlot();
    }

    public void UnequipItem(int index)
    {
        slots[index].ClearSlot();
        player.UnequipItem(index);
    }

    public void UpdatePlayerStatusInfo(Status level, Status equipment)
    {
        damage.text = "<color=green>" + level.attack.ToString() + "</color>" + "\t" + "  +  " + "\t" + "<color=blue>" + equipment.attack.ToString() + "</color>";
        defence.text = "<color=green>" + level.defence.ToString() + "</color>" + "\t" + "  +  " + "\t" + "<color=blue>" + equipment.defence.ToString();
        recoverySP.text = "<color=green>" + level.recoverySP.ToString() + "</color>" + "\t" + "  +  " + "\t" + "<color=blue>" + equipment.recoverySP.ToString() + "</color>";
        recoveryMP.text = "<color=green>" + level.recoveryMP.ToString() + "</color>" + "\t" + "  +  " + "\t" + "<color=blue>" + equipment.recoveryMP.ToString() + "</color>";
        guardGauge.text = "<color=green>" + level.guardGauge.ToString() + "</color>" + "\t" + "  +  " + "\t" + "<color=blue>" + equipment.guardGauge.ToString() + "</color>";
    }

    public void UpdatePlayerLevelInfo(int level, float EXP, float nextLevelEXP)
    {
        this.exp.text = "EXP : " + EXP.ToString() + " / " + nextLevelEXP.ToString();
        this.level.text = "Level : " + level.ToString();
    }

    public void LevelUP(float maxHP)
    {
        this.maxHP = maxHP;
    }

    public void UpdateHP(float hp)
    {
        this.hp.text = "<color=red>" + hp.ToString() + " / " + maxHP.ToString() + "</color>";
    }

    public void UpdateMP(float mp)
    {
        this.mp.text = "<color=blue>" + mp.ToString() + " / " + maxMP.ToString() + "</color>";
    }

    public void UpdateSP(float sp)
    {
        this.sp.text = "<color=yellow>" + ((int)sp).ToString() + " / " + maxSP.ToString() + "</color>";
    }

    private void OnEnable()
    {
        SoundMgr.Instance.PlaySFXAudio("UI_Open");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
