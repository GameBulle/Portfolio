using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject<T>
{
    void AddObserver(T o);
    void RemoveObserver(T o);
    void UpdateQuest(int questID);
    void UpdateMonster(int monsterID);
    void UpdateItem(DropItem drop);
}
