using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveClearResultUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
    
    [Header("DefenceLine")]
    [SerializeField] DefenceLine defenceLine;

    [Header("Wave")]
    [SerializeField] TextMeshProUGUI wave;

    [Header("Dead Monster")]
    [SerializeField] TextMeshProUGUI deadMonster;

    [Header("Defence Life")]
    [SerializeField] TextMeshProUGUI defenceLineHP;

    [Header("Lost Defence Life")]
    [SerializeField] TextMeshProUGUI lostDefenceLineHP;

    [Header("Get Stuff")]
    [SerializeField] TextMeshProUGUI getStuff;

    public void WaveClear()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("{0} Wave Clear", GameMgr.Instance.Wave);
        wave.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("처치한 몬스터 수 : {0}", GameMgr.Instance.WaveMonster);
        deadMonster.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("현재 방어선 체력 : {0}", defenceLine.HP);
        defenceLineHP.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("손실된 방어선 체력 : {0}", defenceLine.lostHP);
        lostDefenceLineHP.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("뼈 조각 : {0}", ItemMgr.Instance.WaveBone);
        strBuilder.AppendFormat("   ");
        strBuilder.AppendFormat("쇠 조각 : {0}", ItemMgr.Instance.WaveIron);
        strBuilder.AppendFormat("   ");
        strBuilder.AppendFormat("암흑물질 : {0}", ItemMgr.Instance.WaveDarkMaterial);
        getStuff.text = strBuilder.ToString();

        SoundMgr.Instance.WaveClearSoundPlay();
    }
}
