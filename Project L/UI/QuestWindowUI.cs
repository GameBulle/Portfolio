using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class QuestWindowUI : MakeSlot
{
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] TextMeshProUGUI rewardGold;
    [SerializeField] TextMeshProUGUI target;
    [SerializeField] RectTransform textContent;
    [SerializeField] GameObject temp;
    [SerializeField] GameObject acceptButton;
    [SerializeField] GameObject cancelButton;

    TextAsset textAsset;
    //float textHeight;
    int questID;

    void Init()
    {
        questName.text = string.Empty;
        questText.text = string.Empty;
        rewardGold.text = string.Empty;
        target.text = string.Empty;
    }

    public void QuestWindow(int questID, bool detail = false)
    {
        Init();
        if(detail)
        {
            acceptButton.SetActive(false);
            cancelButton.SetActive(true);
        }
        else
        {
            acceptButton.SetActive(true);
            cancelButton.SetActive(false);
        }

        textContent.sizeDelta = Vector2.zero;
        gameObject.SetActive(true);
        TextToParent();

        StringBuilder sb = new();
        List<Dictionary<string, string>> data = CSV.Read("Quest");
        //textHeight = textContent.sizeDelta.y;
        this.questID = questID;

        sb.Append("Text/Quest/Detail/" + questID);
        textAsset = Resources.Load<TextAsset>(sb.ToString());

        questText.text = textAsset.text;
        questText.ForceMeshUpdate(true);
        SetTextContentHeight();

        sb.Clear();
        for (int i=0;i<data.Count;i++)
        {
            if (data[i]["QuestID"] != questID.ToString())
                continue;

            questName.text = data[i]["QuestName"];
            rewardGold.text = data[i]["RewardGold"];
            string[] monsterIDs = data[i]["TargetMonsterID"].Split(";");
            string[] monsterAmounts = data[i]["TargetMonsterAmount"].Split(";");
            string[] itemIDs = data[i]["TargetItemID"].Split(";");
            string[] itemAmounts = data[i]["TargetItemAmount"].Split(";");
            string[] rewardItemIDs = data[i]["RewardItemID"].Split(';');
            string[] rewardItemAmounts = data[i]["RewardItemAmount"].Split(";");
            string etc = data[i]["ETC"];

            if (!string.IsNullOrEmpty(monsterIDs[0]))
            {
                sb.Clear();
                sb.Append("목표 : ");
                for (int m = 0; m < monsterIDs.Length; m++)
                    sb.Append(MonsterMgr.Instance.GetMonsterData(int.Parse(monsterIDs[m])).MonsterName + " x " + monsterAmounts[m] + "   ");
                sb.Append("\n");
                target.text += sb.ToString();
            }

            if (!string.IsNullOrEmpty(itemIDs[0]))
            {
                sb.Clear();
                for (int t = 0; t < itemIDs.Length; t++)
                    sb.Append("목표 : " + ItemMgr.Instance.GetItem(int.Parse(itemIDs[t])).ItemName + " x " + itemAmounts[t] + "   ");
                sb.Append("\n");
                target.text += sb.ToString();
            }

            if(rewardItemIDs.Length > 4)
                SetRowColumn(rewardItemIDs.Length, 1);
            MakeSlotInit(SlotType.Reward);

            for (int s = 0; s < rewardItemIDs.Length; s++) 
                slots[s].SetSlot(ItemMgr.Instance.GetItem(int.Parse(rewardItemIDs[s])), int.Parse(rewardItemAmounts[s]));

            if (!string.IsNullOrEmpty(etc))
                target.text += etc;
        }
    }

    void SetTextContentHeight()
    {
        questText.ForceMeshUpdate(true);
        TMP_TextInfo textInfo = questText.textInfo;
        float height = textInfo.lineInfo[0].lineExtents.max.y - textInfo.lineInfo[textInfo.lineCount - 1].lineExtents.min.y;

        textContent.sizeDelta = new Vector2(textContent.sizeDelta.x, height);
        TextToContent();
    }

    public void ClickAccept()
    {
        QuestMgr.Instance.AcceptQuest(questID);
        SoundMgr.Instance.PlaySFXAudio("QuestAccept");
        gameObject.SetActive(false);
    }

    public void ClickCancel()
    {
        SoundMgr.Instance.PlaySFXAudio("QuestCancel");
        QuestMgr.Instance.CancelQuest(questID);
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        gameObject.SetActive(false);
    }

    void TextToParent()
    {
        questText.transform.SetParent(temp.transform, false);
    }

    void TextToContent()
    {
        questText.transform.SetParent(textContent.transform, false);
    }

    private void OnEnable()
    {
        SoundMgr.Instance.PlaySFXAudio("UI_Open");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
