using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour, IAchieveSubject
{
    public enum Unlock { UnlockSamurai, UnlockShaman }

    Unlock[] Unlocks;
    List<IAchieveObserver> observers;
    Dictionary<string, int> check_unlock;

    private void Awake()
    {
        observers = new();
        check_unlock = new();
        Unlocks = (Unlock[])Enum.GetValues(typeof(Unlock));
    }

    private void LateUpdate()
    {
        foreach (Unlock unlock in Unlocks)
            CheckUnlock(unlock);
    }

    public void Initialize()
    {
        check_unlock.Add("Init", 1);
        foreach (Unlock unlock in Unlocks)
            check_unlock.Add(unlock.ToString(), 0);
    }

    public void AddObserver(IAchieveObserver o)
    {
        if (observers.Find(x => x == o) == null)
            observers.Add(o);
    }

    public void RemoveObserver(IAchieveObserver o)
    {

    }

    public void UpdateUnlock(string name, bool isLoad)
    {
        for (int i = 0; i < observers.Count; i++)
            observers[i].UpdateAchieve(name, isLoad);
    }

    void CheckUnlock(Unlock achive)
    {
        bool isUnlock = false;
        string char_name = "";
        switch (achive)
        {
            case Unlock.UnlockSamurai:
                char_name = "사무라이";
                isUnlock = GameManager.Instance.GameTime >= 360f;
                break;
            case Unlock.UnlockShaman:
                char_name = "샤먼";
                isUnlock = GameManager.Instance.GoldInGame >= 50;
                break;
        }

        if (isUnlock && check_unlock[achive.ToString()] == 0)
        {
            check_unlock[achive.ToString()] = 1;
            UpdateUnlock(char_name, false);
            GameManager.Instance.SaveGameData();
        }
    }

    public void LoadData(List<string> unlock_name, List<int> unlock_value)
    {
        for (int i = 0; i < unlock_name.Count; i++)
        {
            check_unlock[unlock_name[i]] = unlock_value[i];
            if (unlock_value[i] == 1 && unlock_name[i] != "Init")
                UpdateUnlock(unlock_name[i], true);
        }
    }

    public Dictionary<string, int> GetCurrAchieve() { return check_unlock; }
}
