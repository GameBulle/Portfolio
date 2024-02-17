using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [Header("Skill Area")]
    [SerializeField] SkillLevelUI skillLevelUIPrefab;

    [Header("Horizon")]
    [SerializeField] float horizon;

    [Header("Vertical")]
    [SerializeField] float vertical;

    [SerializeField] RectTransform contentArea;
    [SerializeField] TextMeshProUGUI skillPointText;

    Player player;
    public int SkillPoint { get; set; }
    protected SkillLevelUI[] skills;

    public void Initialize(Player player)
    {
        int size = SkillMgr.Instance.SkillCount;
        this.player = player;

        //SkillPoint = player.Level;
        SkillPoint = 10;
        for (int i = 0; i < SkillMgr.Instance.SkillCount; i++)
            SkillPoint -= SkillMgr.Instance.GetSkill(i).Level;

        skills = new SkillLevelUI[size];

        Vector2 startPos = new Vector2(vertical, -horizon);
        Vector2 curPos = startPos;

        for (int i = 0; i < size; i++) 
        {
            SkillLevelUI skill = Instantiate(skillLevelUIPrefab);
            RectTransform rt = skill.GetComponent<RectTransform>();
            rt.SetParent(contentArea);

            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = curPos;
            rt.gameObject.SetActive(true);
            rt.gameObject.name = $"Skill Area [{i}]";

            skill.Index = i;
            skill.Initialize(MakeSlot.SlotType.SkillUI, SkillMgr.Instance.GetSkill(i), this);
            skills[i] = skill;

            if(i >= 2)
                contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x + rt.sizeDelta.x + 15, contentArea.sizeDelta.y);

            curPos.x += rt.sizeDelta.x + horizon;
        }

        //SkillPoint = player.Level;
        skillPointText.text = SkillPoint.ToString();
        gameObject.SetActive(false);
    }

    public void UpdateSkillPoint() 
    { 
        skillPointText.text = SkillPoint.ToString(); 
    }

    public void PlayerLevelUP()
    {
        SkillPoint++;
        UpdateSkillPoint();
    }

    public void InitSkills() 
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        for (int i = 0; i < skills.Length; i++) 
        {
            SkillMgr.Instance.SkillInit(i,0);
            SkillPoint += skills[i].Level;
            skills[i].SkillInit();
        }
        UpdateSkillPoint();
        InterfaceMgr.Instance.InitSkills(); 
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
