using TMPro;
using UnityEngine;
using System.IO;
using System.Text;

public class LoadSlot : MonoBehaviour
{
    [SerializeField] GameObject empty;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI saveTime;
    [SerializeField] TextMeshProUGUI playTime;

    int index;

    public void Initilize(int index)
    {
        this.index = index;
    }


    public void UpdateLoadSlotUI(PlayerSaveData data)
    {
        empty.SetActive(false);
        StringBuilder sb = new();

        sb.Append("Level : ");
        sb.Append(data.Level);
        level.text = sb.ToString();

        sb.Clear();
        sb.Append("마지막 시간 : ");
        sb.Append(data.SaveTime);
        saveTime.text = sb.ToString();

        sb.Clear();
        sb.Append("플레이 시간 : ");
        sb.Append(data.PlayTime);
        playTime.text = sb.ToString();
    }

    public void Click()
    {
        if (empty.activeSelf == true)
            return;
        SoundMgr.Instance.PlaySFXAudio("GameStart");
        DataMgr.Instance.LoadData(index);
        InterfaceMgr.Instance.GameStart();
    }

    private void OnEnable()
    {
        StringBuilder sb = new();
        sb.Append(DataMgr.Instance.Path);
        sb.Append(index);
  
        if (File.Exists(sb.ToString()))
        {
            string loadFile = File.ReadAllText(sb.ToString());
            PlayerSaveData loadData = JsonUtility.FromJson<PlayerSaveData>(loadFile);
            UpdateLoadSlotUI(loadData);
        }
        else
            empty.SetActive(true);
    }
}
