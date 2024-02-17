using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "ScriptableObject/NPC/NPC", order = 0)]
public class NPCData : ScriptableObject
{
    [SerializeField] int npcID;
    [SerializeField] string npcName;
    [SerializeField] Sprite npcImage;
    [SerializeField] QuestData questData = null;
    

    public int NpcID => npcID;
    public string NPCName => npcName;
    public Sprite NPCImage => npcImage;
    public QuestData QuestData => questData;
}
