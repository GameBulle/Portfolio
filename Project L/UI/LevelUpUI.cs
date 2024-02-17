using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;

    public void SetLevelUpUI(int level)
    {
        gameObject.SetActive(true);
        StringBuilder sb = new();
        sb.Append("Level : ");
        sb.Append(level.ToString());
        levelText.text = sb.ToString();
        StartCoroutine(OnUI());
    }

    IEnumerator OnUI()
    {
        yield return new WaitForSecondsRealtime(3f);

        gameObject.SetActive(false);
    }
}
