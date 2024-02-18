using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeBowUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("Text")]
    [SerializeField] TextMeshProUGUI bowName;
    [SerializeField] TextMeshProUGUI timeBetShot;
    [SerializeField] TextMeshProUGUI chargeSpeed;
    [SerializeField] TextMeshProUGUI shotAngle;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI stuff;
    [SerializeField] TextMeshProUGUI page;

    [Header("IconObject")]
    [SerializeField] GameObject iconObject;
    Image iconImage;

    int currPage = 0;
    int maxPage = 0;
    BowData bowData;

    public void Initialize()
    {
        currPage = 1;
        maxPage = ItemMgr.Instance.MaxBowData - 1;
        iconImage = iconObject.GetComponent<Image>();

        DisPlayInfo();
    }

    public void DisPlayInfo()
    {
        bowData = ItemMgr.Instance.GetBowData(currPage);

        strBuilder.Clear();
        strBuilder.Append(bowData.name);
        bowName.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("»ç°Ý ¼Óµµ : {0}", bowData.TimeBetShot);
        timeBetShot.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("Â÷Â¡ ¼Óµµ : {0}", bowData.ChargeSpeed);
        chargeSpeed.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("»ç°Ý °¢µµ : {0}", bowData.ShotAngle);
        shotAngle.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("¼Óµµ : {0}", bowData.Speed);
        speed.text = strBuilder.ToString();

        DisplayCount();

        strBuilder.Clear();
        strBuilder.AppendFormat("»À Á¶°¢ : {0}", bowData.Bone);
        strBuilder.AppendFormat("  ");
        strBuilder.AppendFormat("¼è Á¶°¢ : {0}", bowData.Iron);
        strBuilder.AppendFormat("  ");
        strBuilder.AppendFormat("¾ÏÈæ¹°Áú : {0}", bowData.DarkMaterial);
        stuff.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.Append(currPage);
        page.text = strBuilder.ToString();

        iconImage.sprite = bowData.Icon;
    }

    public void DisplayCount()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("°¹¼ö : {0}", ItemMgr.Instance.GetCount(bowData.ID));
        count.text = strBuilder.ToString();
    }

    public void Make()
    {
        if (ItemMgr.Instance.Bone >= bowData.Bone &&
            ItemMgr.Instance.Iron >= bowData.Iron &&
            ItemMgr.Instance.DarkMaterial >= bowData.DarkMaterial)
        {
            ItemMgr.Instance.PlusItem(bowData.ID);
            ItemMgr.Instance.Bone -= bowData.Bone;
            ItemMgr.Instance.Iron -= bowData.Iron;
            ItemMgr.Instance.DarkMaterial -= bowData.DarkMaterial;

            SoundMgr.Instance.MakeSoundPlay();
        }

        DisplayCount();
    }

    public void Next()
    {
        if (currPage == maxPage)
            return;

        currPage++;
        DisPlayInfo();
    }

    public void Prev()
    {
        if (currPage == 1)
            return;

        currPage--;
        DisPlayInfo();
    }
}
