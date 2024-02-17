using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ShopPopUpUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;

    public void PopUpActivate(int itemID)
    {
        gameObject.SetActive(true);
        StringBuilder sb = new();
        sb.Append("<color=blue>");
        sb.Append(ItemMgr.Instance.GetItem(itemID).ItemName);
        sb.AppendLine("</color>를(을) ");
        sb.Append("구매 하시겠습니까?");
        itemName.text = sb.ToString();
    }

    public void Disabled()
    {
        gameObject.SetActive(false);
    }
}
