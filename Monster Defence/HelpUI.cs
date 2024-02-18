using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("Page")]
    [SerializeField] GameObject[] pageObjects;

    [Header("PageCount")]
    [SerializeField] TextMeshProUGUI pageCount;

    [Header("Buttons")]
    [SerializeField] GameObject[] buttons;

    enum Button { Next,Prev}

    int page;

    public void Initialize()
    {
        page = 1;

        for (int i = 0; i < pageObjects.Length; i++)
            pageObjects[i].SetActive(false);

        pageCount.enabled = true;

        for(int i=0;i<buttons.Length;i++)
            buttons[i].SetActive(true);

        DisplayPage();
    }

    void DisplayPage()
    {
        if(page == 1)
        {
            buttons[(int)Button.Prev].SetActive(false);
        }

        if(page == pageObjects.Length)
        {
            pageCount.enabled = false;
            buttons[(int)Button.Next].SetActive(false);
        }

        pageObjects[page - 1].SetActive(true);

        strBuilder.Clear();
        strBuilder.AppendFormat("{0}", page);
        pageCount.text= strBuilder.ToString();
    }

    public void ClickNextButton()
    {
        if(page==1)
            buttons[(int)Button.Prev].SetActive(true);
        pageObjects[page - 1].SetActive(false);
        page++;

        SoundMgr.Instance.PaperFlipSoundPlay();
        DisplayPage();
    }

    public void ClickPrevButton()
    {
        if (page == pageObjects.Length)
        {
            pageCount.enabled = true;
            buttons[(int)Button.Next].SetActive(true);
        }
        pageObjects[page - 1].SetActive(false);
        page--;

        SoundMgr.Instance.PaperFlipSoundPlay();
        DisplayPage();
    }
}
