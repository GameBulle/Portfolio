using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestObserver
{
    public abstract void UpdateQuest(int questID);
    public abstract void UpdateMonster(int monsterID);
    public abstract void UpdateItem(DropItem item);
}
