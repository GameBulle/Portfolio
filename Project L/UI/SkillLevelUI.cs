using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillLevelUI : MakeSlot
{
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillLevel;

    [Header("Disabled Skill")]
    [SerializeField] Image disabledImage;

    SkillUI skillUI;
    public int Index { get; set; }
    public int Level { get; set; }

    public void Initialize(SlotType type, SkillData skillData, SkillUI skillUI)
    {
        MakeSlotInit(type);
        slots[0].Index = this.Index;
        slots[0].SetSlot(skillData);
        skillName.text = skillData.SkillName;
        Level = skillData.Level;
        UpdateSkillLevel();
        this.skillUI = skillUI;
        disabledImage.transform.SetAsLastSibling();
    }

    void UpdateSkillLevel()
    {
        skillLevel.text = Level.ToString();
    }

    void UpdateSkillTooltip()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].SkillToolTipUpdate(SkillMgr.Instance.GetSkill(Index));
    }

    public void SkillLevelUP()
    {
        if(skillUI.SkillPoint >=1)
        {
            SoundMgr.Instance.PlaySFXAudio("Click");
            Level++;
            skillUI.SkillPoint--;
            skillUI.UpdateSkillPoint();
            UpdateSkillLevel();
            SkillMgr.Instance.SkillLevelUP(Index);
            UpdateSkillTooltip();
            disabledImage.gameObject.SetActive(false);
        }
    }

    public void SkillLevelDown()
    {
        if(Level > 0)
        {
            SoundMgr.Instance.PlaySFXAudio("Click");
            Level--;
            skillUI.SkillPoint++;
            skillUI.UpdateSkillPoint();
            UpdateSkillLevel();
            SkillMgr.Instance.SkillLevelDown(Index);
            UpdateSkillTooltip();
            if (Level == 0)
                disabledImage.gameObject.SetActive(true);
        }
    }

    public void SkillInit()
    {
        Level = 0;
        disabledImage.gameObject.SetActive(true);
        UpdateSkillLevel();
        UpdateSkillTooltip();
    }

    private void OnEnable()
    {
        if(Level > 0)
            disabledImage.gameObject.SetActive(false);
    }
}
