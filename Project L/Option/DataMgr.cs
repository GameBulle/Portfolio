using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using Unity.VisualScripting;
using System.Text;

[System.Serializable]
public class PlayerSaveData
{
    public int Level;
    public float HP;
    public float MP;
    public float SP;
    public float EXP;
    public int MapID;
    public int Gold;
    public Vector3 PlayerPos;
    public Vector3 PlayerAngle;
    public string SaveTime;
    public float PlayTime;
    public int[] equipItems;
    public DropItem[] Items;
    public DropItem[] itemSlot;
    public int[] SkillSlots;
    public int[] SkillLevels;
}

[System.Serializable]
public class OptionSaveData
{
    public int FPS;
    public FullScreenMode screenMode;
    public OptionUI.RESOLUTION resolution;
    public float sensivity;
    public float masterVolume;
    public float backgroundVolume;
    public float SFXVolume;
}

public class DataMgr : MonoBehaviour
{
    public string Path { get; private set; }

    static DataMgr instance = null;
    public static DataMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataMgr>();
                if(!instance)
                    instance = new GameObject("DataManger").AddComponent<DataMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        StringBuilder sb = new();
        if (this != Instance)
            Destroy(gameObject);
        sb.Append(Application.persistentDataPath);
        sb.Append("/");
        sb.Append("Save");
        Path = sb.ToString();
    }

    public void SavePlayerData(int index, PlayerSaveData data)
    {
        StringBuilder sb = new();
        string saveFile = JsonUtility.ToJson(data, true);
        sb.Append(Path);
        sb.Append(index);
        File.WriteAllText(sb.ToString(), saveFile);
    }

    public bool LoadData(int index)
    {
        StringBuilder sb = new();
        sb.Append(Path);
        sb.Append(index);
        if (!File.Exists(sb.ToString()))
            return false;
        string loadFile = File.ReadAllText(sb.ToString());
        PlayerSaveData loadData = JsonUtility.FromJson<PlayerSaveData>(loadFile);
        SceneLoader.Instance.StartGame(loadData);
        return true;
    }
}
