using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gold_text;

    private void OnEnable()
    {
        StringBuilder sb = new();
        sb.Append("»πµÊ«— ∞ÒµÂ∑Æ : ");
        sb.Append(GameManager.Instance.GoldInGame.ToString());
        gold_text.text = sb.ToString();
    }
}
