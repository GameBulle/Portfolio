using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAchieveSubject
{
    void AddObserver(IAchieveObserver o);
    void RemoveObserver(IAchieveObserver o);
    void UpdateUnlock(string name, bool isLoad);
}
