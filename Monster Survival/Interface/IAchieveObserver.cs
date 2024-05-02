using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAchieveObserver
{
    void UpdateAchieve(string name, bool isLoad);
}
