using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNPC : NPC, IQuestObserver
{
    [SerializeField] Image questIcon;

    protected override void Awake()
    {
        base.Awake();

        questIcon.sprite = Resources.Load<Sprite>("Icon/Quest_before");
        IQuestObserver observer = this.gameObject.GetComponent<IQuestObserver>();
        QuestMgr.Instance.AddObserver(observer);
        if (npcData.QuestData.AntecedenceQuestID != -1)
            questIcon.gameObject.SetActive(false);
    }

    public void UpdateQuest(int questID)
    {
        CheckLinkQuestID(questID);

        if (npcData.QuestData.ID == questID)
        {
            switch (QuestMgr.Instance.GetQuestData(questID).State)
            {
                case QuestMgr.QuestState.Before:
                    questIcon.sprite = Resources.Load<Sprite>("Icon/Quest_before");
                    break;
                case QuestMgr.QuestState.Proceed:
                    questIcon.sprite = Resources.Load<Sprite>("Icon/Quest_proceed");
                    break;
                case QuestMgr.QuestState.Complete:
                    questIcon.sprite = Resources.Load<Sprite>("Icon/Quest_complete");
                    break;
                case QuestMgr.QuestState.End:
                    questIcon.gameObject.SetActive(false);
                    break;
            }
        }
    }

    void CheckLinkQuestID(int questID)
    {
        if (questID == npcData.QuestData.AntecedenceQuestID && 
            QuestMgr.Instance.GetQuestData(questID).State == QuestMgr.QuestState.End)
            questIcon.gameObject.SetActive(true);
    }

    public void UpdateMonster(int monsterID)
    {

    }

    public void UpdateItem(DropItem item)
    {

    }
}
