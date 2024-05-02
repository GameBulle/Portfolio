using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IAchieveObserver
{
    [SerializeField] CharacterData[] datas;

    IAchieveObserver observer;

    public int Size => datas.Length;

    private void Awake()
    {
        observer = this.gameObject.GetComponent<IAchieveObserver>();
        GameManager.Instance.AddAchieveObserver(observer);
    }

    public void Initialize()
    {
        for (int i = (int)CharacterData.CharacterType.Samurai; i < datas.Length; i++)
            datas[i].IsLock = true;

        for(int i=0;i<datas.Length;i++)
            datas[i].Upgrade.InitLevel(0, 0, 0, 0);
    }

    public void UpdateAchieve(string name, bool isLoad)
    {
        int index = Array.FindIndex(datas, x => x.CharacterName == name);
        datas[index].IsLock = false;
        if (!isLoad)
            InterfaceManager.Instance.SetAlarmData(datas[index]);
    }

    public void LoadData(List<string> character_name, List<int> character_level)
    {
        int j = 0;
        for (int i = 0; i < character_name.Count; i++)
        {
            int index = Array.FindIndex(datas, x => x.CharacterName == character_name[i]);
            datas[index].Upgrade.InitLevel(character_level[j++], character_level[j++], character_level[j++], character_level[j++]);
        }
    }

    public CharacterData GetCharacterData(int index) { return datas[index]; }
    public CharacterData[] GetAllData() { return datas; }
}
