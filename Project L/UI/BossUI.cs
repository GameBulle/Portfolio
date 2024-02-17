using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossName;
    [SerializeField] Slider bossHP;
    [SerializeField] Slider bossGrogyGauge;


    public void BossUIActivate(string name, float maxHp, float grogyGauge)
    {
        gameObject.SetActive(true);
        bossName.text = name;
        bossHP.maxValue = maxHp;
        bossHP.value = maxHp;
        bossGrogyGauge.maxValue = grogyGauge;
        bossGrogyGauge.value = grogyGauge;
    }

    public void UpdateHP(float hp)
    {
        bossHP.value = hp;
    }

    public void UpdateGrogyGauge(float gauge)
    {
        bossGrogyGauge.value = gauge;
    }
}
