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
        strBuilder.AppendFormat("óġ�� ���� �� : {0}", GameMgr.Instance.WaveMonster);
        deadMonster.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("���� �� ü�� : {0}", defenceLine.HP);
        defenceLineHP.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("�սǵ� �� ü�� : {0}", defenceLine.lostHP);
        lostDefenceLineHP.text = strBuilder.ToString();

        strBuilder.Clear();
        strBuilder.AppendFormat("�� ���� : {0}", ItemMgr.Instance.WaveBone);
        strBuilder.AppendFormat("   ");
        strBuilder.AppendFormat("�� ���� : {0}", ItemMgr.Instance.WaveIron);
        strBuilder.AppendFormat("   ");
        strBuilder.AppendFormat("���湰�� : {0}", ItemMgr.Instance.WaveDarkMaterial);
        getStuff.text = strBuilder.ToString();

        SoundMgr.Instance.WaveClearSoundPlay();
    }
}
