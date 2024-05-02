using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    StringBuilder sb;
    string path;

    private void Awake()
    {
        sb = new();
        sb.Append(Application.persistentDataPath);
        sb.Append("/Save");
        path = sb.ToString();
    }

    public void SaveGameData(CharacterData[] charcater_data, Dictionary<string, int> achieve_data, int gold)
    {
        sb.Clear();
        GameData save_data = new();
        save_data.gold = gold;
        for (int i = 0; i < charcater_data.Length; i++)
        {
            save_data.character_name.Add(charcater_data[i].CharacterName);
            save_data.character_level.Add(charcater_data[i].Upgrade.GetMoveSpeedLevel());
            save_data.character_level.Add(charcater_data[i].Upgrade.GetHPLevel());
            save_data.character_level.Add(charcater_data[i].Upgrade.GetMPPlusLevel());
            save_data.character_level.Add(charcater_data[i].Upgrade.GetSkillDamageLevel());
        }

        foreach(KeyValuePair<string,int> achieve in achieve_data)
        {
            save_data.achieve_name.Add(achieve.Key);
            save_data.achieve_value.Add(achieve.Value);
        }

        string save_file = JsonUtility.ToJson(save_data, true);
        sb.Append(path);
        sb.Append("_GameData");
        File.WriteAllText(sb.ToString(), save_file);
    }

    public void SaveOptionData(float masterVolume, float backgroundVolume, float SFXVolume, int FPS)
    {
        sb.Clear();
        OptionData option_data = new();
        option_data.masterVolume = masterVolume;
        option_data.backgroundVolume = backgroundVolume;
        option_data.SFXVolume = SFXVolume;
        option_data.FPS = FPS;

        string save_file = JsonUtility.ToJson(option_data, true);
        sb.Append(path);
        sb.Append("_OptionData");
        File.WriteAllText(sb.ToString(), save_file);
    }

    public GameData LoadGameData()
    {
        sb.Clear();
        sb.Append(path);
        sb.Append("_GameData");
        if (!File.Exists(sb.ToString()))
            return null;
        string load_data_file = File.ReadAllText(sb.ToString());
        GameData data = JsonUtility.FromJson<GameData>(load_data_file);
        return data;
    }

    public OptionData LoadOptionData()
    {
        sb.Clear();
        sb.Append(path);
        sb.Append("_OptionData");
        if (!File.Exists(sb.ToString()))
            return null;
        string load_data = File.ReadAllText(sb.ToString());
        OptionData data = JsonUtility.FromJson<OptionData>(load_data);
        return data;
    }
}

[System.Serializable]
public class GameData
{
    public List<string> character_name = new();
    public List<int> character_level = new();

    public List<string> achieve_name = new();
    public List<int> achieve_value = new();

    public int gold;
}

[System.Serializable]
public class OptionData
{
    public float masterVolume;
    public float backgroundVolume;
    public float SFXVolume;
    public int FPS;
}