using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPlayerUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("Player")]
    [SerializeField] Player player;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI playerBow;
    [SerializeField] TextMeshProUGUI playerArrow1;
    [SerializeField] TextMeshProUGUI playerArrow2;

    [Header("Info")]
    [SerializeField] BowInfo bowInfo;
    [SerializeField] ArrowInfo arrowInfo;

    [Header("Input")]
    [SerializeField] GameObject Input;

    [Header("Icon Object")]
    [SerializeField] GameObject bowIcon;
    [SerializeField] GameObject arrow1Icon;
    [SerializeField] GameObject arrow2Icon;
    Image bowIconImage;
    Image arrow1IconImage;
    Image arrow2IconImage;

    enum ItemCase { Bow,Arrow1,Arrow2 }
    ItemCase itemCase { get; set; }

    private void Awake()
    {
        bowIconImage = bowIcon.GetComponent<Image>();
        arrow1IconImage = arrow1Icon.GetComponent<Image>();
        arrow2IconImage = arrow2Icon.GetComponent<Image>();
    }

    public void Initialize()
    {
        bowInfo.gameObject.SetActive(true);
        bowInfo.Initialize();

        arrowInfo.gameObject.SetActive(false);
        Input.gameObject.SetActive(false);

        PlayerBow();
        PlayerArrow1();
        PlayerArrow2();
    }

    public void PlayerBow()
    {
        strBuilder.Clear();
        strBuilder.Append(player.bowData.ItemName);
        playerBow.text = strBuilder.ToString();

        bowIconImage.sprite = player.bowData.Icon;
    }

    public void ClickBow()
    {
        bowIconImage.sprite = player.bowData.Icon;

        itemCase = ItemCase.Bow;
        bowInfo.gameObject.SetActive(true);
        bowInfo.Initialize();

        arrowInfo.gameObject.SetActive(false);
    }

    public void PlayerArrow1()
    {
        arrow1IconImage.sprite = player.Arrows[0].Icon;

        strBuilder.Clear();
        strBuilder.Append(player.Arrows[0].ItemName);
        strBuilder.AppendLine();
        if (player.Arrows[0].ID == ItemMgr.Instance.Arrow_Start_ID)
            strBuilder.Append(" x ¡Ä");
        else
            strBuilder.AppendFormat(" x {0}", player.ArrowCount[0]);
        playerArrow1.text = strBuilder.ToString();
    }

    public void ClickArrow1()
    {
        itemCase = ItemCase.Arrow1;
        arrowInfo.gameObject.SetActive(true);
        arrowInfo.Initialize();

        bowInfo.gameObject.SetActive(false);
    }

    public void PlayerArrow2()
    {
        arrow2IconImage.sprite = player.Arrows[1].Icon;

        strBuilder.Clear();
        strBuilder.Append(player.Arrows[1].ItemName);
        strBuilder.AppendLine();
        if (player.Arrows[1].ID == ItemMgr.Instance.Arrow_Start_ID)
            strBuilder.Append("x ¡Ä");
        else
            strBuilder.AppendFormat("x {0}", player.ArrowCount[1]);
        playerArrow2.text = strBuilder.ToString();
    }

    public void ClickArrow2()
    {
        itemCase= ItemCase.Arrow2;
        arrowInfo.gameObject.SetActive(true);
        arrowInfo.Initialize();

        bowInfo.gameObject.SetActive(false);
    }

    public void ClickBowMount()
    {
        if (CheckPlayerItem())
        {
            SoundMgr.Instance.EquipSoundPlay();
            player.MountBow(bowInfo.Mount());
            PlayerBow();
        }
    }

    public void ClickArrowMount()
    {
        if(CheckPlayerItem())
            Input.SetActive(true);
    }

    public bool CheckPlayerItem()
    {
        int id = 0;
        switch(itemCase)
        {
            case ItemCase.Bow:
                id = player.bowData.ID;
                if (id == bowInfo.GetBowID())
                    return false;
                else
                    return true;
            case ItemCase.Arrow1:
                id = player.Arrows[0].ID;
                break;
            case ItemCase.Arrow2:
                id = player.Arrows[1].ID;
                break;
        }

        if (id == arrowInfo.GetArrowID()) 
            return false;

        if(arrowInfo.GetArrowID() == ItemMgr.Instance.Arrow_Start_ID)
        {
            switch(itemCase)
            {
                case ItemCase.Arrow1:
                    player.MountArrow1(arrowInfo.GetNormalArrow());
                    PlayerArrow1();
                    break;
                case ItemCase.Arrow2:
                    player.MountArrow2(arrowInfo.GetNormalArrow());
                    PlayerArrow2();
                    break;
            }
            SoundMgr.Instance.EquipSoundPlay();
            return false;
        }


        return true;
    }

    public void ClickYes()
    {
        switch(itemCase)
        {
            case ItemCase.Arrow1:
                player.MountArrow1(arrowInfo.Mount(), arrowInfo.ArrowCount());
                PlayerArrow1();
                break;
            case ItemCase.Arrow2:
                player.MountArrow2(arrowInfo.Mount(), arrowInfo.ArrowCount());
                PlayerArrow2();
                break;
        }
        SoundMgr.Instance.EquipSoundPlay();
        Input.SetActive(false);
    }

    public void ClickNo()
    {
        Input.SetActive(false);
    }
}
