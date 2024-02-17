using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : Spawner<QuestUI>
{
    [SerializeField] Vector2 margin;
    [SerializeField] Transform temp;

    List<QuestUI> quests;

    public void Initialize()
    {
        InitializeSpawner();
        quests = new();
    }

    public void AddQuest(int questID)
    {
        Vector2 Pos = new Vector2(10f, -(margin.y));
        QuestUI quest = Spawn(0, Pos);
        quest.gameObject.SetActive(true);

        if(quests.Count >= 1)
            quest.transform.SetParent(quests[quests.Count - 1].transform, false);
        else
            quest.transform.SetParent(gameObject.transform, false);

        quest.AddQuest(questID);
        quest.Index = quests.Count;
        quests.Add(quest);
        quest.OnReturnEvent += (quest) => RemoveQuestUI(quest.Index);
        quest.OnReturnEvent += (quest) => GiveBackItem(quest);
    }

    public void RemoveQuestUI(int index)
    {
        if (index == quests.Count - 1)
        {
            quests[index].transform.SetParent(temp, false);
            quests.RemoveAt(index);
        }
        else
        {
            if (index - 1 >= 0)
                quests[index + 1].transform.SetParent(quests[index - 1].transform, false);
            else
                quests[index + 1].transform.SetParent(temp.transform, false);
            quests.RemoveAt(index);
        }
    }
}
