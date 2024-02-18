using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DefenceLineUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("DefenceLine")]
    [SerializeField] DefenceLine defenceLine;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI maxHP;
    [SerializeField] TextMeshProUGUI HP;
    [SerializeField] TextMeshProUGUI Level;
    [SerializeField] TextMeshProUGUI LevelUpStuff;
    [SerializeField] TextMeshProUGUI RepairStuff;

    int level = 0;

    int levelUpBone = 0;
    int levelUpIron = 0;
    int levelUpDarkMaterial = 0;

    int repairBone = 0;
    int repairIron = 0;
    int repairDarkMaterial = 0;

    public void Initialize()
    {
        NeedLevelupStuff();
        NeedRepairStuff();

        DisplayDefenceLineInfo();
        DisplayNeedStuff();
    }

    void NeedLevelupStuff()
    {
        level = defenceLine.Level;
        levelUpBone = level * 10;
        levelUpIron = level * 8;
        levelUpDarkMaterial = level * 1;
    }

    void NeedRepairStuff()
    {
        repairBone = level * 3;
        repairIron = level * 2;
        repairDarkMaterial = level % 10;
    }

    public void DisplayDefenceLineInfo()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("방어선 최대 체력 : {0}", defenceLine.MaxHP);
        maxHP.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("방어선 현재 체력 : {0}", defenceLine.HP);
        HP.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("방어선 Level : {0}", defenceLine.Level);
        Level.text = strBuilder.ToString();
    }

    public void DisplayNeedStuff()
    {
        // 레벨업 재료
        strBuilder.Clear();
        strBuilder.AppendFormat("뼈 조각 : {0}", levelUpBone);
        strBuilder.AppendLine();
        strBuilder.AppendFormat("쇠 조각 : {0}", levelUpIron);
        strBuilder.AppendLine();
        strBuilder.AppendFormat("암흑 물질 : {0}", levelUpDarkMaterial);
        LevelUpStuff.text = strBuilder.ToString();

        // 수리 재료
        strBuilder.Clear();
        strBuilder.AppendFormat("뼈 조각 : {0}", repairBone);
        strBuilder.AppendLine();
        strBuilder.AppendFormat("쇠 조각 : {0}", repairIron);
        strBuilder.AppendLine();
        strBuilder.AppendFormat("암흑 물질 : {0}", repairDarkMaterial);
        RepairStuff.text = strBuilder.ToString();
    }

    public bool LevelUpCheck()
    {
        if (ItemMgr.Instance.Bone >= levelUpBone &&
            ItemMgr.Instance.Iron >= levelUpIron &&
            ItemMgr.Instance.DarkMaterial >= levelUpDarkMaterial)
            return true;

        return false;
    }

    public bool RepairCheck()
    {
        if ((ItemMgr.Instance.Bone >= repairBone &&
            ItemMgr.Instance.Iron >= repairIron &&
            ItemMgr.Instance.DarkMaterial >= repairDarkMaterial) &&
            defenceLine.HP < defenceLine.MaxHP) 
            return true;

        return false;
    }

    public void LevelUp()
    {
        if(LevelUpCheck())
        {
            defenceLine.LevelUp();
            ItemMgr.Instance.Bone -= levelUpBone;
            ItemMgr.Instance.Iron -= levelUpIron;
            ItemMgr.Instance.DarkMaterial -= levelUpDarkMaterial;

            NeedLevelupStuff();
            SoundMgr.Instance.LevelUpSoundPlay();
        }
            
        DisplayDefenceLineInfo();
        DisplayNeedStuff();
    }

    public void Reapair()
    {
        if(RepairCheck())
        {
            defenceLine.RestoreHealth(10);
            ItemMgr.Instance.Bone -= repairBone;
            ItemMgr.Instance.Iron -= repairIron;
            ItemMgr.Instance.DarkMaterial -= repairDarkMaterial;

            NeedRepairStuff();
            SoundMgr.Instance.RepairSoundPlay();
        }
            
        DisplayDefenceLineInfo();
        DisplayNeedStuff();
    }
}
