using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindSurvivorUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("NPCManager")]
    [SerializeField] NPCMgr nPCMgr = null;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI currSurvivorCount;
    [SerializeField] TextMeshProUGUI maxSurvivorCount;
    [SerializeField] TextMeshProUGUI findPercentage;
    [SerializeField] TextMeshProUGUI needStuff;
    [SerializeField] TextMeshProUGUI cantFind;

    float percent = 0.0f;

    public void Initialize()
    {
        GetCurrSurvivor();
        GetMaxSurvivor();

        if (CheckSurvivor())
        {
            findPercentage.gameObject.SetActive(false);
            needStuff.gameObject.SetActive(false);
            cantFind.gameObject.SetActive(true);

            CantFind();
        }
        else
        {
            findPercentage.gameObject.SetActive(true);
            needStuff.gameObject.SetActive(true);
            cantFind.gameObject.SetActive(false);

            GetFindPercentage();
            GetNeedStuff();
        }
    }

    public void GetCurrSurvivor()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("���� ������ �� : {0}", nPCMgr.NPCs);
        currSurvivorCount.text = strBuilder.ToString();
    }

    public void GetMaxSurvivor()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("���� ������ �ִ� ������ �� : {0}", nPCMgr.MaxNPC);
        maxSurvivorCount.text = strBuilder.ToString();
    }

    public void GetFindPercentage()
    {
        percent = (1f - ((float)nPCMgr.NPCs / (float)nPCMgr.MaxNPC)) * 100f;
        strBuilder.Clear();
        strBuilder.AppendFormat("Ž�� ���� Ȯ�� : {0}%", percent);
        findPercentage.text = strBuilder.ToString();
    }

    public void GetNeedStuff()
    {
        strBuilder.Clear();
        strBuilder.Append("������ Ž���� �ʿ��� ���");
        strBuilder.AppendLine();
        strBuilder.AppendFormat("�� ���� : {0}", nPCMgr.NPCs * 10);
        strBuilder.AppendLine();
        strBuilder.AppendFormat("�� ���� : {0}", nPCMgr.NPCs * 10);
        strBuilder.AppendLine();
        strBuilder.AppendFormat("���湰�� : {0}", nPCMgr.NPCs * 5);
        needStuff.text = strBuilder.ToString();
    }

    public void CantFind()
    {
        strBuilder.Clear();
        strBuilder.Append("�� �̻� �����ڸ� Ž�� �� �� �����ϴ�.");
        cantFind.text = strBuilder.ToString();
    }

    public void ClickFind()
    {
        if ((ItemMgr.Instance.Bone >= nPCMgr.NPCs * 10 &&
            ItemMgr.Instance.Iron >= nPCMgr.NPCs * 10 &&
            ItemMgr.Instance.DarkMaterial >= nPCMgr.NPCs * 5) && !CheckSurvivor())
        {
            ItemMgr.Instance.Bone -= nPCMgr.NPCs * 10;
            ItemMgr.Instance.Iron -= nPCMgr.NPCs * 10;
            ItemMgr.Instance.DarkMaterial -= nPCMgr.NPCs * 5;

            if (CheckPercent())
            {
                nPCMgr.PlusNPC();
                Initialize();
                SoundMgr.Instance.HiSoundPlay();
            }
        }
    }

    public bool CheckPercent()
    {
        return (Random.value * 100 <= percent) ;
    }

    public bool CheckSurvivor()
    {
        return (nPCMgr.NPCs >= nPCMgr.MaxNPC);
    }
}
