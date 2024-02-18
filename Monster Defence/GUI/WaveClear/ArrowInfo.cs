using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowInfo : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("Text")]
    [SerializeField] TextMeshProUGUI arrowName;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI penetration;
    [SerializeField] TextMeshProUGUI maxPenetration;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI page;

    [Header("Input")]
    [SerializeField] TMP_InputField input;

    [Header("Icon Object")]
    [SerializeField] GameObject iconObject;
    Image iconImage;

    int currPage = 0;
    int maxPage = 0;
    int mountCount = 0;
    public int Count { get; set; }
    List<ArrowData> arrows;

    private void Awake()
    {
        iconImage = iconObject.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if(input.text.Length >= 1)
        {
            if(Count < int.Parse(input.text))
            {
                input.text = Count.ToString();
            }
        }
    }

    public void Initialize()
    {
        currPage = 1;
        arrows = ItemMgr.Instance.GetArrows();
        maxPage = arrows.Count;
        DisplayInfo();
    }

    public void DisplayInfo()
    {
        strBuilder.Clear();
        strBuilder.Append(arrows[currPage-1].name);
        arrowName.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("공격력 : {0}", arrows[currPage - 1].Damage);
        damage.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("기본 관툥력 : {0}", arrows[currPage - 1].Penetration);
        penetration.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("최대 관통력 : {0}", arrows[currPage - 1].MaxPenetration);
        maxPenetration.text = strBuilder.ToString();

        DisplayCount();

        strBuilder.Clear();
        strBuilder.Append(currPage);
        page.text = strBuilder.ToString();

        iconImage.sprite = arrows[currPage - 1].Icon;
    }

    public void DisplayCount()
    {
        Count = ItemMgr.Instance.GetCount(arrows[currPage - 1].ID);
        if (Count < 0)
            Count = 0;
        strBuilder.Clear();
        strBuilder.AppendFormat("갯수 : {0}", Count);
        countText.text = strBuilder.ToString();
    }

    public ArrowData GetNormalArrow()
    {
        return arrows[currPage - 1];
    }

    public ArrowData Mount()
    {
        mountCount = int.Parse(input.text);
        if (ItemMgr.Instance.ArrowMountCheck(arrows[currPage - 1].ID, mountCount))
        {
            DisplayCount();
            input.text = "1";
            return arrows[currPage - 1];
        }

        return null;
    }

    public int ArrowCount()
    {
        return mountCount;
    }

    public int GetArrowID()
    {
        return arrows[currPage - 1].ID;
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
