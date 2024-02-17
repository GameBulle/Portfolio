using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class TalkUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI talkerName;
    [SerializeField] TextMeshProUGUI talkText;
    [SerializeField] Image npcImage;
    [SerializeField] Image playerImage;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject clickImage;

    enum TextContent { Intro, Talk, Quest, QuestClear}
    enum Button { Button_Start, Exit = Button_Start, Talk, Quest, QuestClear, Button_End = QuestClear}

    string text;
    TextAsset textAsset;
    NPCData npcData;
    bool inputCheck;

    IEnumerator test(TextContent textContent)
    {
        StringBuilder sb = new();
        switch (textContent)
        {
            case TextContent.Intro:
                sb.Append("Text/NPC/Intro/" + npcData.NpcID);
                textAsset = Resources.Load<TextAsset>(sb.ToString());
                break;

            case TextContent.Talk:
                sb.Append("Text/NPC/Talk/" + npcData.NpcID);
                textAsset = Resources.Load<TextAsset>(sb.ToString());
                break;

            case TextContent.Quest:
                sb.Append("Text/Quest/Intro/" + npcData.QuestData.ID);
                textAsset = Resources.Load<TextAsset>(sb.ToString());
                break;

            case TextContent.QuestClear:
                sb.Append("Text/Quest/QuestClear/" + npcData.QuestData.ID);
                textAsset = Resources.Load<TextAsset>(sb.ToString());
                break;
        }

        text = textAsset.text;
        var splits = text.Split("\n");
        for (int i = 0; splits.Length > i; i++)
        {
            talkText.text = string.Empty;
            switch (splits[i])
            {
                case "0\r":
                    npcImage.gameObject.SetActive(false);
                    playerImage.gameObject.SetActive(true);
                    talkerName.text = "Player";
                    continue;

                case "1\r":
                    if(npcData.NPCImage != null)
                        npcImage.gameObject.SetActive(true);
                    playerImage.gameObject.SetActive(false);
                    talkerName.text = npcData.NPCName;
                    continue;
            }

            yield return DisplayText(splits[i]);

            while (true)
            {
                if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Mouse0)) && !inputCheck)) 
                    break;
                yield return null;

                inputCheck = false;
            }
        }

        switch(textContent)
        {
            case TextContent.Intro:
                int end;
                if (npcData.NPCName == "Pointer")
                    end = (int)Button.Exit;
                else if (QuestCheck())
                    end = (int)Button.Quest;
                else if ((npcData.QuestData != null && QuestMgr.Instance.GetQuestData(npcData.QuestData.ID).State == QuestMgr.QuestState.Complete))
                    end = (int)Button.QuestClear;
                else if (npcData.QuestData == null)
                    end = (int)Button.Talk;
                else
                    end = (int)Button.Talk;

                for (int start = 0; start <= end; start++)
                    buttons[start].gameObject.SetActive(true);
                break;

            case TextContent.Talk:
                ClickExit();
                break;

            case TextContent.Quest:
                InterfaceMgr.Instance.QuestWindow(npcData.QuestData.ID);
                ClickExit();
                break;

            case TextContent.QuestClear:
                QuestMgr.Instance.QuestClear(npcData.QuestData.ID);
                ClickExit();
                break;
        }

        clickImage.gameObject.SetActive(false);

    }

    bool QuestCheck()
    {
        if(npcData.QuestData != null && QuestMgr.Instance.GetQuestData(npcData.QuestData.ID).State == QuestMgr.QuestState.Before)
        {
            if (npcData.QuestData.AntecedenceQuestID == -1)
                return true;
            else if (QuestMgr.Instance.GetQuestData(npcData.QuestData.AntecedenceQuestID).State == QuestMgr.QuestState.End)
                return true;
            else
                return false;
        }
        return false;
    }

    public void ClickExit()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        GameMgr.Instance.IsTalk = false;
        gameObject.SetActive(false);
    }

    public void ClickTalk()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        DisabledButtons();
        StartCoroutine(test(TextContent.Talk));
        clickImage.gameObject.SetActive(true);
    }

    public void ClickQuest()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        DisabledButtons();
        StartCoroutine(test(TextContent.Quest));
        clickImage.gameObject.SetActive(true);
    }

    public void ClickQuestClear()
    {
        SoundMgr.Instance.PlaySFXAudio("QuestClear");
        DisabledButtons();
        StartCoroutine(test(TextContent.QuestClear));
        clickImage.gameObject.SetActive(true);
    }

    IEnumerator DisplayText(string text)
    {
        int i = 0;
        float timer = 0.1f;
        System.Text.StringBuilder sb = new();

        while(text.Length > i)
        {
            if(0 >= timer)
            {
                timer += 0.1f;

                sb.Append(text[i++]);
                talkText.text = sb.ToString();
            }

            yield return null;
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                inputCheck = true;
                break;
            }
            timer -= Time.deltaTime;
        }
        talkText.text = text;
    }

    public void TalkInfo(NPCData npcData)
    {
        gameObject.SetActive(true);
        clickImage.gameObject.SetActive(true);
        DisabledButtons();
        talkText.text = string.Empty;
        this.npcData = npcData;
        if(this.npcData.NPCImage != null)
            npcImage.sprite = this.npcData.NPCImage;
        else
            npcImage.sprite = Resources.Load<Sprite>("Icon/NPC/Nope");

        GameMgr.Instance.IsTalk = true;
        StartCoroutine(test(TextContent.Intro));
    }

    void DisabledButtons()
    {
        for (int i = (int)Button.Button_Start; i <= (int)Button.Button_End; i++)
            buttons[i].gameObject.SetActive(false);
    }

    private void OnEnable()
    {
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
