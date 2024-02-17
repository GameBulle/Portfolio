using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Data", menuName = "ScriptableObject/Quest/Quest", order = 0)]
public class QuestData : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] int antecedenceQuestID = -1;

    public QuestMgr.QuestState State { get; set; }

    private void Awake()
    {
        State = QuestMgr.QuestState.Before;
    }

    public int ID => id;
    public int AntecedenceQuestID => antecedenceQuestID;
}
