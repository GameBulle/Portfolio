using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMgr : MonoBehaviour, ISubject<IQuestObserver>
{
    [SerializeField] QuestData[] quests;
    [SerializeField] int maxQuest;

    List<int> acceptQuests;
    List<IQuestObserver> observers;
    int listIndex = -1;
    Player player;
    public enum QuestState { Before, Proceed , Complete, End }
    public int QuestSize { get { return maxQuest; } }

    static QuestMgr instance = null;
    public static QuestMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestMgr>();
                if(!instance)
                    instance = new GameObject("QuestManager").AddComponent<QuestMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
        acceptQuests = new();
        observers = new();
    }

    public void Initialize(Player player)
    {
        this.player = player;
        Array.Sort(quests, (a, b) => (a.ID < b.ID) ? -1 : 1);
        
        for (int i = 0; i < quests.Length; i++) 
            quests[i].State= QuestState.Before;
    }

    public QuestData GetQuestData(int questID) { return quests[questID]; }

    public void AcceptQuest(int questID)
    {
        if (acceptQuests.Count < maxQuest)
        {
            quests[questID].State = QuestState.Proceed;
            acceptQuests.Add(questID);
            InterfaceMgr.Instance.AddQuest(questID);
            UpdateQuest(questID);
        }
    }

    public void CancelQuest(int questID)
    {
        quests[questID].State = QuestState.Before;
        acceptQuests.Remove(questID);
        UpdateQuest(questID);
    }

    public void QuestComplete(int questID)
    {
        quests[questID].State = QuestState.Complete;
        UpdateQuest(questID);
    }

    public void QuestClear(int questID)
    {
        quests[questID].State = QuestState.End;
        acceptQuests.Remove(questID);
        UpdateQuest(questID);
        RewardToPlayer(questID);
    }

    void RewardToPlayer(int questID)
    {
        List<DropItem> reward = new();
        List<Dictionary<string, string>> data = CSV.Read("Quest");
        for(int i=0;i<data.Count;i++)
        {
            if (data[i]["QuestID"] != questID.ToString())
                continue;

            string[] rewardItems = data[i]["RewardItemID"].Split(";");
            string[] rewardAmount = data[i]["RewardItemAmount"].Split(";");

            for(int j=0;j<rewardItems.Length;j++)
            {
                DropItem item = new DropItem();
                item.id = int.Parse(rewardItems[j]); 
                item.amount = int.Parse(rewardAmount[j]);
                reward.Add(item);
            }
        }

        player.GetItems(reward);
    }

    public void AddObserver(IQuestObserver o)
    {
        if (observers.Find(x => x == o) == null)
        {
            observers.Add(o);
            listIndex++;
        }
    }

    public void RemoveObserver(IQuestObserver o)
    {
        observers.Remove(o);
        listIndex--;
    }

    public void UpdateQuest(int questID)
    {
        for (int i = 0; i <= listIndex; i++)
            observers[i].UpdateQuest(questID);
    }

    public void UpdateMonster(int monsterID)
    {
        for (int i = 0; i <= listIndex; i++)
            observers[i].UpdateMonster(monsterID);
    }

    public void UpdateItem(DropItem item)
    {
        for (int i = 0; i <= listIndex; i++)
            observers[i].UpdateItem(item);
    }
}
