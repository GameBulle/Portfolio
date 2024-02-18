using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeArrowUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("Text")]
    [SerializeField] TextMeshProUGUI arrowName;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI penetration;
    [SerializeField] TextMeshProUGUI maxPenetration;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI stuff;
    [SerializeField] TextMeshProUGUI page;

    [Header("IconObject")]
    [SerializeField] GameObject iconObject;
    Image iconImage;

    int currPage = 0;
    int maxPage = 0;
    ArrowData arrowData;

    public void Initialize()
    {
        currPage = 1;
        maxPage = ItemMgr.Instance.MaxArrowData - 1;
        iconImage = iconObject.GetComponent<Image>();

        DisplayInfo();
    }

    public void DisplayInfo()
    {
        arrowData = ItemMgr.Instance.GetArrowData(currPage);

        strBuilder.Clear();
        strBuilder.Append(arrowData.name);
        arrowName.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("°ø°Ý·Â : {0}", arrowData.Damage);
        damage.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("±âº» °ü¸œ·Â : {0}", arrowData.Penetration);
        penetration.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("ÃÖ´ë °üÅë·Â : {0}", arrowData.MaxPenetration);
        maxPenetration.text = strBuilder.ToString();

        DisplayCount();

        strBuilder.Clear();
        strBuilder.AppendFormat("»À Á¶°¢ : {0}", arrowData.Bone);
        strBuilder.AppendFormat("  ");
        strBuilder.AppendFormat("¼è Á¶°¢ : {0}", arrowData.Iron);
        strBuilder.AppendFormat("  ");
        strBuilder.AppendFormat("¾ÏÈæ¹°Áú : {0}", arrowData.DarkMaterial);
        stuff.text= strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.Append(currPage);
        page.text = strBuilder.ToString();

        iconImage.sprite = arrowData.Icon;
    }

    public void DisplayCount()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("°¹¼ö : {0}", ItemMgr.Instance.GetCount(arrowData.ID));
        count.text = strBuilder.ToString();
    }

    public void Make()
    {
        if (ItemMgr.Instance.Bone >= arrowData.Bone &&
            ItemMgr.Instance.Iron >= arrowData.Iron &&
            ItemMgr.Instance.DarkMaterial >= arrowData.DarkMaterial)
        {
            ItemMgr.Instance.PlusItem(arrowData.ID);
            ItemMgr.Instance.Bone -= arrowData.Bone;
            ItemMgr.Instance.Iron -= arrowData.Iron;
            ItemMgr.Instance.DarkMaterial -= arrowData.DarkMaterial;

            SoundMgr.Instance.MakeSoundPlay();
        }
        
        DisplayCount();
    }

    public void Next()
    {
        if (currPage == maxPage)
            return;

        currPage++;
        DisplayInfo();
    }

    public void Prev()
    {
        if (currPage == 1)
            return;

        currPage--;
        DisplayInfo();
    }
}
