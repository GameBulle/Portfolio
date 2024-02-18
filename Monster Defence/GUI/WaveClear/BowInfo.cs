using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BowInfo : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("Text")]
    [SerializeField] TextMeshProUGUI bowName;
    [SerializeField] TextMeshProUGUI timeBetShot;
    [SerializeField] TextMeshProUGUI chargeSpeed;
    [SerializeField] TextMeshProUGUI shotAngle;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI page;

    [Header("Icon Object")]
    [SerializeField] GameObject iconObject;
    Image iconImage;

    int currPage = 0;
    int maxPage = 0;
    List<BowData> bows;

    private void Awake()
    {
        iconImage = iconObject.GetComponent<Image>();
    }

    public void Initialize()
    {
        currPage = 1;

        bows = ItemMgr.Instance.GetBows();
        maxPage = bows.Count;
        DisPlayInfo();
    }

    public void DisPlayInfo()
    {
        strBuilder.Clear();
        strBuilder.Append(bows[currPage - 1].name);
        bowName.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("사격 속도 : {0}", bows[currPage - 1].TimeBetShot);
        timeBetShot.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("차징 속도 : {0}", bows[currPage - 1].ChargeSpeed);
        chargeSpeed.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("사격 각도 : {0}", bows[currPage - 1].ShotAngle);
        shotAngle.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("속도 : {0}", bows[currPage - 1].Speed);
        speed.text = strBuilder.ToString();

        DisplayCount();

        strBuilder.Clear();
        strBuilder.Append(currPage);
        page.text = strBuilder.ToString();

        iconImage.sprite = bows[currPage - 1].Icon;
    }

    public void DisplayCount()
    {
        int Count = ItemMgr.Instance.GetCount(bows[currPage - 1].ID);
        if (Count < 0)
            Count = 0;
        strBuilder.Clear();
        strBuilder.AppendFormat("갯수 : {0}", Count);
        count.text = strBuilder.ToString();
    }

    public int GetBowID()
    {
        return bows[currPage - 1].ID;
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

    public BowData Mount()
    {
        if (ItemMgr.Instance.BowMountCheck(bows[currPage - 1].ID))
        {
            DisplayCount();
            return bows[currPage - 1];
        }
            
        return null;
    }
}
