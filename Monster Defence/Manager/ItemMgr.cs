using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour
{
    static ItemMgr instance = null;
    public static ItemMgr Instance
    {
        get
        {
            if(null == instance)
            {
                instance = FindObjectOfType<ItemMgr>();
                if (!instance)
                    instance = new GameObject("ItemManager").AddComponent<ItemMgr>();

                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    enum Item
    {
        // Arrow
        Arrow_Start = 0,
        Normal_Arrow = Arrow_Start,
        Bone_Arrow,
        Iron_Arrow,
        Iron_Bonw_Arrow,
        Elf_Arrow,
        Arrow_End,
        // Bow
        Bow_Start = Arrow_End,
        Normal_Bow = Bow_Start,
        Bone_Bow,
        Iron_Bow,
        Elf_Bow,
        Bow_End
    }

    public int Arrow_Start_ID => (int)Item.Arrow_Start;
    public int Arrow_End_ID => (int)Item.Arrow_End;
    public int Bow_Start_ID => (int)Item.Bow_Start;
    public int Bow_End_ID => (int)Item.Bow_End;

    SortedDictionary<int, int> inventory = new SortedDictionary<int, int>();

    [Header("Arrow")]
    [SerializeField] ArrowData[] arrowDatas;

    public int MaxArrowData => arrowDatas.Length;

    [Header("Bow")]
    [SerializeField] BowData[] bowDatas;

    public int MaxBowData => bowDatas.Length;

    // 드랍 재료 갯수
    public int Bone { get; set; }
    public int WaveBone { get; set; }
    public int Iron { get; set; }
    public int WaveIron { get; set; }
    public int DarkMaterial { get; set; }
    public int WaveDarkMaterial { get; set; }

    public int GetCount(int id)
    {
        if (inventory.ContainsKey(id))
            return inventory[id];
        else
            return 0;
    }

    public void PlusItem(int id, int count = 1)
    {
        if (id == (int)Item.Bow_Start || id == (int)Item.Arrow_Start)
            return;

        if (inventory.ContainsKey(id))
            inventory[id] += count;
        else
            inventory.Add(id, count);
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    void Initialize()
    {
        Array.Sort(arrowDatas, (a, b) => (a.ID < b.ID) ? -1 : 1);
        Array.Sort(bowDatas, (a, b) => (a.ID < b.ID) ? -1 : 1);
        inventory.Add((int)Item.Normal_Arrow, -1);
        inventory.Add((int)Item.Normal_Bow, -1);
    }

    public void DropStuff(int bone, int iron, int dark)
    {
        WaveBone += bone;
        WaveIron += iron;
        WaveDarkMaterial += dark;
    }

    //public void MakeArrow(int index)
    //{
    //    if (CheckStuff(index, true)) 
    //    {
    //        bonePiece -= arrowData[index].Bone;
    //        ironPiece -= arrowData[index].Iron;
    //        darkMaterial -= arrowData[index].DarkMaterial;
    //        arrowCount[index]++;
    //    }
    //}

    //public void MakeBow(int index)
    //{
    //    if (CheckStuff(index, false)) 
    //    {
    //        bonePiece -= bowData[index].Bone;
    //        ironPiece -= bowData[index].Iron;
    //        darkMaterial -= bowData[index].DarkMaterial;
    //        bowCount[index]++;
    //    }
    //}

    //bool CheckStuff(int index, bool type)
    //{
    //    if(type)
    //    {
    //        if (bonePiece >= arrowData[index].Bone &&
    //            ironPiece >= arrowData[index].Iron &&
    //            darkMaterial >= arrowData[index].DarkMaterial)
    //            return true;
    //    }
    //    else
    //    {
    //        if (bonePiece >= bowData[index].Bone &&
    //            ironPiece >= bowData[index].Iron &&
    //            darkMaterial >= bowData[index].DarkMaterial)
    //            return true;
    //    }

    //    return false;
    //}
    public void WaveStart()
    {
        WaveBone = 0;
        WaveIron = 0;
        WaveDarkMaterial= 0;
    }

    public void WaveClear()
    {
        Bone += WaveBone;
        Iron += WaveIron;
        DarkMaterial += WaveDarkMaterial;
    }

    public ArrowData GetArrowData(int id)
    {
        return arrowDatas[id];
    }

    public BowData GetBowData(int id)
    {
        return bowDatas[id];
    }

    public List<ArrowData> GetArrows()
    {
        List<ArrowData> arrows = new List<ArrowData>();
        foreach(KeyValuePair<int,int> pair in inventory)
        {
            if (pair.Key < (int)Item.Arrow_End && pair.Key >= (int)Item.Arrow_Start)
            {
                int index = Array.FindIndex(arrowDatas, element => element.ID == pair.Key);
                arrows.Add(arrowDatas[index]);
            }
            else
                continue;
        }
        return arrows;
    }

    public List<BowData> GetBows()
    {
        List<BowData> bows = new List<BowData>();
        foreach(KeyValuePair<int,int> pair in inventory)
        {
            if (pair.Key < (int)Item.Bow_End && pair.Key >= (int)Item.Bow_Start)
            {
                int index = Array.FindIndex(bowDatas, element => element.ID == pair.Key);
                bows.Add(bowDatas[index]);
            }
            else
                continue;
        }
        return bows;
    }

    public bool BowMountCheck(int id)
    {
        if (id == (int)Item.Bow_Start)
            return true;

        if (inventory[id] > 0)
        {
            inventory[id]--;
            if (inventory[id] == 0)
                inventory.Remove(id);
            return true;
        }

        return false;
    }

    public bool ArrowMountCheck(int id, int count = 0)
    {
        if (count == 0)
            return false;

        if (id == (int)Item.Arrow_Start)
            return true;

        if(inventory[id] >= count)
        {
            inventory[id] -= count;
            if (inventory[id] == 0)
                inventory.Remove(id);
            return true;
        }

        return false;
    }
}
