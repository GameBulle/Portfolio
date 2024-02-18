using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingNPCUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("No NPC TEXT")]
    [SerializeField] TextMeshProUGUI bowNoNPC;
    [SerializeField] TextMeshProUGUI arrowNoNPC;

    [Header("NPC Equipment")]
    [SerializeField] TextMeshProUGUI bow;
    [SerializeField] TextMeshProUGUI arrow;

    [Header("Info")]
    [SerializeField] ArrowInfo arrowInfo;
    [SerializeField] BowInfo bowInfo;

    [Header("Input")]
    [SerializeField] GameObject input;

    [Header("Page")]
    [SerializeField] TextMeshProUGUI page;

    [Header("NPCManager")]
    [SerializeField] NPCMgr npcMgr;

    [Header("Icon Object")]
    [SerializeField] GameObject bowIcon;
    [SerializeField] GameObject arrowIcon;
    Image bowIconImage;
    Image arrowIconImage;

    enum ItemCase { Bow,Arrow}
    ItemCase itemCase { get; set; }

    int index;

    private void Awake()
    {
        bowIconImage = bowIcon.GetComponent<Image>();
        arrowIconImage = arrowIcon.GetComponent<Image>();
    }

    public void Initialize()
    {
        index = 0;
        Page();

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 curr = rectTransform.anchoredPosition;

        if (CheckNPC())
        {
            bowNoNPC.enabled = false;
            arrowNoNPC.enabled = false;

            bow.enabled = true;
            arrow.enabled = true;
            bowIcon.SetActive(true);
            arrowIcon.SetActive(true);

            bowInfo.gameObject.SetActive(true);
            bowInfo.Initialize();

            NPCBow();
            NPCArrow();

            rectTransform.anchoredPosition = new Vector2(-125, curr.y);
        }
        else
        {
            bowNoNPC.enabled = true;
            arrowNoNPC.enabled = true;

            bowInfo.gameObject.SetActive(false);
            bow.enabled = false;
            arrow.enabled = false;
            bowIcon.SetActive(false);
            arrowIcon.SetActive(false);
            rectTransform.anchoredPosition = new Vector2(0, curr.y);
        }
            
        arrowInfo.gameObject.SetActive(false);
        input.gameObject.SetActive(false);
    }
    
    public bool CheckNPC()
    {
        return (npcMgr.NPCs > 0);
    }

    public void Page()
    {
        strBuilder.Clear();
        strBuilder.Append(index+1);
        page.text = strBuilder.ToString();
    }

    public void NPCBow()
    {
        bow.enabled = true;
        bowIconImage.sprite = npcMgr.GetNPCbow(index).Icon;

        strBuilder.Clear();
        strBuilder.Append(npcMgr.GetNPCbow(index).ItemName);
        bow.text = strBuilder.ToString();
    }

    public void ClickBow()
    {
        if(CheckNPC())
        {
            itemCase = ItemCase.Bow;
            bowInfo.gameObject.SetActive(true);
            bowInfo.Initialize();

            arrowInfo.gameObject.SetActive(false);
        }
    }

    public void NPCArrow()
    {
        arrow.enabled = true;
        arrowIconImage.sprite = npcMgr.GetNPCarrow(index).Icon;

        strBuilder.Clear();
        strBuilder.Append(npcMgr.GetNPCarrow(index).ItemName);
        strBuilder.AppendLine();
        if (npcMgr.GetNPCarrow(index).ID == ItemMgr.Instance.Arrow_Start_ID)
            strBuilder.Append("x ¡Ä");
        else
            strBuilder.AppendFormat("x {0}", npcMgr.GetNPCarrowCount(index));
        arrow.text = strBuilder.ToString();
    }

    public void ClickArrow()
    {
        if(CheckNPC())
        {
            itemCase = ItemCase.Arrow;
            arrowInfo.gameObject.SetActive(true);
            arrowInfo.Initialize();

            bowInfo.gameObject.SetActive(false);
        }
    }

    public void ClickNext()
    {
        if (index >= npcMgr.NPCs - 1)
            return;

        index++;
        NPCBow();
        NPCArrow();
        Page();
    }

    public void ClickPrev()
    {
        if (index == 0)
            return;

        index--;
        NPCBow();
        NPCArrow();
        Page();
    }

    public void ClickBowMount()
    {
        if(CheckNPCItem())
        {
            SoundMgr.Instance.EquipSoundPlay();
            npcMgr.SetNPCbow(index, bowInfo.Mount());
            NPCBow();
        }
    }

    public void ClickArrowMount()
    {
        if (CheckNPCItem())
            input.SetActive(true);
    }

    public bool CheckNPCItem()
    {
        int id = 0;
        switch(itemCase)
        {
            case ItemCase.Bow:
                id = npcMgr.GetNPCbow(index).ID;
                if (id == bowInfo.GetBowID())
                    return false;
                else
                    return true;
            case ItemCase.Arrow:
                id = npcMgr.GetNPCarrow(index).ID;
                if (id == arrowInfo.GetArrowID())
                    return false;
                else if(arrowInfo.GetArrowID() == ItemMgr.Instance.Arrow_Start_ID)
                {
                    SoundMgr.Instance.EquipSoundPlay();
                    npcMgr.SetNPCarrow(index, arrowInfo.GetNormalArrow(),0);
                    NPCArrow();

                    return false;
                }

                return true;
        }
        return true;
    }

    public void ClickYes()
    {
        SoundMgr.Instance.EquipSoundPlay();
        npcMgr.SetNPCarrow(index, arrowInfo.Mount(), arrowInfo.ArrowCount());
        NPCArrow();

        input.SetActive(false);
    }

    public void ClickNo()
    {
        input.SetActive(false);
    }
}
