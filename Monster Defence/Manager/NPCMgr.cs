using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct WeaponDatas
{
    public BowData bowData;
    public ArrowData arrowData;
    public int arrowCount;
}

public class NPCMgr : Spawner<NPC>
{
    [SerializeField] Transform[] positions = null;
    [SerializeField] int maxNPC = 1;

    List<WeaponDatas> datas;

    public int NPCs { get; private set; }
    public bool IsMax => NPCs >= maxNPC;
    public int MaxNPC => maxNPC;

    public void Initialize()
    {
        NPCs = 0;
        gameObject.SetActive(true);
        datas = new List<WeaponDatas>();

        InitializeSpawner();
    }

    public void PlusNPC()
    {
        NPCs++;

        WeaponDatas data = new WeaponDatas();
        data.bowData = ItemMgr.Instance.GetBowData(0);
        data.arrowData = ItemMgr.Instance.GetArrowData(0);
        data.arrowCount = 0;

        datas.Add(data);
    }

    public void WaveStart()
    {
        SpawnNPC();
        for (int i = 0; i < positions.Length; i++)
            positions[i].gameObject.SetActive(false);
    }

    public void WaveClear()
    {
        for (int i = 0; i < positions.Length; i++)
            positions[i].gameObject.SetActive(true);
        ReturnPool();
    }

    void SpawnNPC()
    {
        NPC npc = null;
        for(int i=0;i<NPCs;i++)
        {
            npc = Spawn(0, positions[i].position);
            npc.NPCInitialize();
            npc.SetData(i);
            npc.MountBow(datas[i].bowData);

            npc.MountArrow1(datas[i].arrowData, datas[i].arrowCount);
            npc.OnReturnBackEvent += (npc) => ReturnBackItem(npc);
        }
    }

    public ArrowData GetNPCarrow(int index)
    {
        return datas[index].arrowData;
    }

    public int GetNPCarrowCount(int index)
    {
        return datas[index].arrowCount;
    }

    public BowData GetNPCbow(int index)
    {
        return datas[index].bowData;
    }

    public void SetNPCbow(int index, BowData bowData)
    {
        if(bowData)
        {
            ItemMgr.Instance.PlusItem(datas[index].bowData.ID);
            WeaponDatas a = datas[index];
            a.bowData = bowData;
            datas[index] = a;
        }
    }

    public void SetNPCarrow(int index, ArrowData arrowData, int count)
    {
        if(arrowData)
        {
            ItemMgr.Instance.PlusItem(datas[index].arrowData.ID, datas[index].arrowCount);
            WeaponDatas a = datas[index];
            a.arrowData = arrowData;
            a.arrowCount = count;
            datas[index] = a;
        }
    }

    void ReturnBackItem(NPC npc)
    {
        WeaponDatas a = datas[npc.Number];
        a.arrowData = npc.Arrows[0];
        a.arrowCount = npc.ArrowCount[0];
        datas[npc.Number] = a;
    }

    public void GameOver()
    {
        ReturnPool();
        gameObject.SetActive(false);
    }
}
