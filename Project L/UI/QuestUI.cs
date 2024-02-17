using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.Xml.Linq;

public class QuestUI : MonoBehaviour,IPoolingObject,IQuestObserver
{
    [SerializeField] RectTransform bg;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] RectTransform content;
    [SerializeField] TextMeshProUGUI target;
    [SerializeField] GameObject questComplete;

    IQuestObserver observer;
    public int QuestID { get; private set; }
    public int Index { get; set; }
    public float ContentHeight => content.sizeDelta.y;
    public event System.Action<QuestUI> OnReturnEvent = null;
    bool isUnfold;
    float bgHeight;
    // <QuestID, <targetID, <targetAmount, currAmount>>>
     Dictionary<int, int[]> targetMonsterCheck;
     Dictionary<int, int[]> targetItemCheck;

    private void Awake()
    {
        targetMonsterCheck = new();
        targetItemCheck = new();
        bgHeight = bg.sizeDelta.y;
    }

    void Init()
    {
        targetMonsterCheck.Clear();
        targetItemCheck.Clear();
        questName.text = string.Empty;
        target.text = string.Empty;
    }

    public void AddQuest(int questID)
    {
        Init();
        observer = this.gameObject.GetComponent<IQuestObserver>();
        QuestMgr.Instance.AddObserver(observer);

        StringBuilder sb = new();
        content.gameObject.SetActive(false);
        questComplete.SetActive(false);
        isUnfold = false;
        content.sizeDelta = new Vector2(content.sizeDelta.x, 50f);
        bg.sizeDelta = new Vector2(bg.sizeDelta.x, bgHeight);
        List<Dictionary<string, string>> data = CSV.Read("Quest");
        QuestID = questID;

        for (int i=0;i<data.Count;i++)
        {
            if (data[i]["QuestID"] != questID.ToString())
                continue;

            questName.text = data[i]["QuestName"];
            string[] monsterIDs = data[i]["TargetMonsterID"].Split(";");
            string[] monsterAmounts = data[i]["TargetMonsterAmount"].Split(";");
            string[] itemIDs = data[i]["TargetItemID"].Split(";");
            string[] itemAmounts = data[i]["TargetItemAmount"].Split(";");
            string etc = data[i]["ETC"];

            if (!string.IsNullOrEmpty(monsterIDs[0]))
            {
                for (int m = 0; m < monsterIDs.Length; m++)
                    targetMonsterCheck.Add(int.Parse(monsterIDs[m]), new int[] { int.Parse(monsterAmounts[m]), 0 });
                content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + 50f);
                UpdateTargetMonster();
            }

            if (!string.IsNullOrEmpty(itemIDs[0]))
            {
                for (int t = 0; t < itemIDs.Length; t++)
                    targetItemCheck.Add(int.Parse(itemIDs[t]), new int[] { int.Parse(itemAmounts[t], 0) });
                content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + 50f);
                UpdateTargetItem();
            }

            if(!string.IsNullOrEmpty(etc))
            {
                sb.Clear();
                content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + 50f);
                if (!string.IsNullOrEmpty(target.text))
                    sb.Append("\n");
                sb.Append(etc);
                target.text = sb.ToString();
            }
        }
    }
    
    void UpdateTargetMonster()
    {
        StringBuilder sb = new();
        sb.Append("ÅðÄ¡ :");
        foreach (KeyValuePair<int, int[]> data in targetMonsterCheck) 
            sb.Append(" " + MonsterMgr.Instance.GetMonsterData(data.Key).MonsterName + " ( " + data.Value[1] + " / " + data.Value[0] + " )    ");
        target.text = sb.ToString();
    }

    void UpdateTargetItem()
    {
        StringBuilder sb = new();
        if (!string.IsNullOrEmpty(target.text))
            sb.Append("\n");
        sb.Append("È¹µæ :");
        foreach (KeyValuePair<int, int[]> data in targetItemCheck)
            sb.Append(" " + ItemMgr.Instance.GetItem(data.Key).ItemName + " ( " + data.Value[1] + " / " + data.Value[0] + " )    ");
        target.text = sb.ToString();
    }

    public void ClickUnfold()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        if(!isUnfold)
        {
            isUnfold = true;
            content.gameObject.SetActive(true);
            bg.sizeDelta = new Vector2(bg.sizeDelta.x, bgHeight + ContentHeight);
        }
        else
        {
            isUnfold = false;
            content.gameObject.SetActive(false);
            bg.sizeDelta = new Vector2(bg.sizeDelta.x, bgHeight);
        }
    }

    public void ClickDetail()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        InterfaceMgr.Instance.QuestWindow(QuestID, true);
    }

    public void SetPosition(Vector3 pos)
    {

    }

    public void SetAngle(Vector3 angle)
    {

    }

    public void UpdateQuest(int questID)
    {
        if(QuestID == questID)
        {
            switch (QuestMgr.Instance.GetQuestData(questID).State)
            {
                case QuestMgr.QuestState.Before:
                case QuestMgr.QuestState.End:
                    QuestMgr.Instance.RemoveObserver(observer);
                    OnReturnEvent?.Invoke(this);
                    OnReturnEvent = null;
                    break;
                case QuestMgr.QuestState.Complete:
                    questComplete.SetActive(true);
                    break;
            }
        }
    }

    public void UpdateMonster(int monsterID)
    {
        if(targetMonsterCheck.ContainsKey(monsterID))
        {
            if(targetMonsterCheck[monsterID][0] != targetMonsterCheck[monsterID][1])
            {
                targetMonsterCheck[monsterID][1]++;
                UpdateTargetMonster();
                CheckQuestComplete();
            }  
        }
    }

    public void UpdateItem(DropItem item)
    {
        if(targetItemCheck.ContainsKey(item.id))
        {
            if(targetItemCheck[item.id][0] != targetItemCheck[item.id][1])
            {
                targetItemCheck[item.id][1]++;
                UpdateTargetItem();
                CheckQuestComplete();
            }
        }
    }

    void CheckQuestComplete()
    {
        bool isComplete = true;
        foreach(KeyValuePair<int, int[]> amount in targetMonsterCheck)
        {
            if (amount.Value[0] != amount.Value[1])
                isComplete = false;
        }

        foreach (KeyValuePair<int, int[]> amount in targetItemCheck)
        {
            if (amount.Value[0] != amount.Value[1])
                isComplete = false;
        }

        if (isComplete)
            QuestMgr.Instance.QuestComplete(QuestID);
    }
}
