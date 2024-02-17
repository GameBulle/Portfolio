using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using System.IO;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI lastTime;
    [SerializeField] TextMeshProUGUI playTime;
    [SerializeField] GameObject empty;

    public event System.Action<SaveSlot> OnClickEvent = null;

    public int Index { get; set; }
    public bool IsEmpty { get; set; }
    public void Initalize(int index)
    {
        Index = index;
    }

    public void UpdateSaveSlotUI(PlayerSaveData data)
    {
        IsEmpty = false;
        empty.SetActive(false);
        StringBuilder sb = new();

        sb.Append("Level : ");
        sb.Append(data.Level);
        level.text = sb.ToString();

        sb.Clear();
        sb.Append("마지막 시간 : ");
        sb.Append(data.SaveTime);
        lastTime.text = sb.ToString();

        sb.Clear();
        sb.Append("플레이 시간 : ");
        sb.Append(data.PlayTime);
        playTime.text = sb.ToString();
    }

    public void ClickSaveSlot()
    {
        OnClickEvent?.Invoke(this);
    }

    private void OnEnable()
    {
        StringBuilder sb = new();
        sb.Append(DataMgr.Instance.Path);
        sb.Append(Index + 1);

        if (File.Exists(sb.ToString()))
        {
            string loadFile = File.ReadAllText(sb.ToString());
            PlayerSaveData loadData = JsonUtility.FromJson<PlayerSaveData>(loadFile);
            UpdateSaveSlotUI(loadData);
        }
        else
        {
            empty.SetActive(true);
            IsEmpty = true;
        }  
    }
}
