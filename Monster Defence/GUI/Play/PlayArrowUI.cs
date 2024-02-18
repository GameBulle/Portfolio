using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayArrowUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("Player")]
    [SerializeField] Player player;

    [Header("Arrow1")]
    [SerializeField] GameObject arrow1;
    [SerializeField] GameObject arrow1Mount;
    [SerializeField] TextMeshProUGUI arrow1Text;
    [SerializeField] TextMeshProUGUI arrow1CountText;
    [SerializeField] GameObject arrow1Icon;
    Image arrow1IconImage;

    [Header("Arrow2")]
    [SerializeField] GameObject arrow2;
    [SerializeField] GameObject arrow2Mount;
    [SerializeField] TextMeshProUGUI arrow2Text;
    [SerializeField] TextMeshProUGUI arrow2CountText;
    [SerializeField] GameObject arrow2Icon;
    Image arrow2IconImage;

    private void Awake()
    {
        arrow1IconImage = arrow1Icon.GetComponent<Image>();
        arrow2IconImage = arrow2Icon.GetComponent<Image>();
    }

    public void WaveStart()
    {
        arrow1Mount.SetActive(false);
        arrow2Mount.SetActive(false);  

        GetArrow1Info();
        GetArrow2Info();
    }

    public void GetArrow1Info()
    {
        strBuilder.Clear();
        strBuilder.Append(player.Arrows[0].ItemName);
        arrow1Text.text = strBuilder.ToString();

        arrow1IconImage.sprite = player.Arrows[0].Icon;

        strBuilder.Clear();
        if (player.Arrows[0].ID == ItemMgr.Instance.Arrow_Start_ID)
            strBuilder.Append("x ¡Ä");
        else
            strBuilder.AppendFormat("x {0}", player.ArrowCount[0]);
        arrow1CountText.text = strBuilder.ToString();
    }

    public void GetArrow2Info()
    {
        strBuilder.Clear();
        strBuilder.Append(player.Arrows[1].ItemName);
        arrow2Text.text = strBuilder.ToString();

        arrow2IconImage.sprite = player.Arrows[1].Icon;

        strBuilder.Clear();
        if (player.Arrows[1].ID == ItemMgr.Instance.Arrow_Start_ID)
            strBuilder.Append("x ¡Ä");
        else
            strBuilder.AppendFormat("x {0}", player.ArrowCount[1]);
        arrow2CountText.text = strBuilder.ToString();
    }

    public void UseArrow1()
    {
        arrow1Mount.SetActive(true);
        arrow2Mount.SetActive(false);
    }

    public void UseArrow2()
    {
        arrow1Mount.SetActive(false);
        arrow2Mount.SetActive(true);
    }

    public void UseNormalArrow()
    {
        arrow1Mount.SetActive(false);
        arrow2Mount.SetActive(false);

        GetArrow1Info();
        GetArrow2Info();
    }
}
