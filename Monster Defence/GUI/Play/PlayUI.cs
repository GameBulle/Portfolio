using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("PlayArrowUI")]
    [SerializeField] PlayArrowUI playArrowUI;

    [Header("WaveText")]
    [SerializeField] TextMeshProUGUI wave;

    [Header("Remain Monster")]
    [SerializeField] TextMeshProUGUI monsterCount;

    [Header("DefenceLine")]
    [SerializeField] DefenceLine defenceLine;

    [Header("HpSlider")]
    [SerializeField] Slider hpSlider;

    public void Initialize()
    {
        SetHpSlider();
    }

    public void WaveStart()
    {
        playArrowUI.gameObject.SetActive(true);
        playArrowUI.WaveStart();
        SetHpSlider();
        Wave();
        MonsterCount();
    }

    public void HpSlider()
    {
        hpSlider.value = defenceLine.HP;
    }

    public void SetHpSlider()
    {
        hpSlider.maxValue = defenceLine.MaxHP;
        hpSlider.value = defenceLine.HP;
    }

    public void UseArrow1()
    {
        playArrowUI.UseArrow1();
    }

    public void UseArrow2()
    {
        playArrowUI.UseArrow2();
    }

    public void UseNormalArrow()
    {
        playArrowUI.UseNormalArrow();
    }

    public void GetArrow1Name()
    {
        playArrowUI.GetArrow1Info();
    }

    public void GetArrow2Name()
    {
        playArrowUI.GetArrow2Info();
    }

    public void Wave()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("Wave : {0}", GameMgr.Instance.Wave);
        wave.text = strBuilder.ToString();
    }

    public void MonsterCount()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("Monster : {0}",GameMgr.Instance.MonsterCount);
        monsterCount.text = strBuilder.ToString();
    }
}
