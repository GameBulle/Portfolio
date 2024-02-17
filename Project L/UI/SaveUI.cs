using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    [SerializeField] GameObject checkDuplication;
    [SerializeField] SaveSlot[] saveSlots;

    int saveIndex;
    Player player;
    public event System.Action<SaveUI> OnSaveEvent = null;

    public void Initialize(Player player)
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            saveSlots[i].Initalize(i);
            saveSlots[i].OnClickEvent += (saveSlot) => CheckDuplication(saveSlot);
        }

        OnSaveEvent += saveUI => Save();
        this.player = player;
        checkDuplication.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void CheckDuplication(SaveSlot slot)
    {
        saveIndex = slot.Index;
        if (!slot.IsEmpty)
            checkDuplication.SetActive(true);
        else
            Save();
    }

    public void ClickYes()
    {
        Save();
        checkDuplication.SetActive(false);
    }

    public void ClickNo()
    {
        checkDuplication.SetActive(false);
    }

    public void Save()
    {
        SoundMgr.Instance.PlaySFXAudio("Save");
        PlayerSaveData data = player.MakeSaveData();
        DataMgr.Instance.SavePlayerData(saveIndex + 1, data);
        UpdateSaveSlotUI(data);
        StartCoroutine(OnSave());
    }

    IEnumerator OnSave()
    {
        InterfaceMgr.Instance.SetTextUI("저장 중...", 1f);

        yield return new WaitForSecondsRealtime(1.0f);

        InterfaceMgr.Instance.SetTextUI("저장 완료", 1f);
    }

    void UpdateSaveSlotUI(PlayerSaveData data)
    {
        saveSlots[saveIndex].UpdateSaveSlotUI(data);
    }

    private void OnEnable()
    {
        SoundMgr.Instance.PlaySFXAudio("UI_Open");
        checkDuplication.SetActive(false);

        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
