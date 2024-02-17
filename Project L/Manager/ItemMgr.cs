using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DropItem
{
    public int id;
    public int amount;
}

[Serializable]
public struct Status
{
    public float attack;
    public float defence;
    public float recoveryMP;
    public float recoverySP;
    public float guardGauge;
}

public class ItemMgr : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] ItemData[] items;

    [Header("DropTable")]
    [SerializeField] ItemDropTable[] dropTables;

    public enum PortionType { HP, MP, SP }
    public enum EquipType { Armor, Amulet, Sword, Helmet, Ring, Shield }

    static ItemMgr instance = null;
    public static ItemMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<ItemMgr>();
                if(!instance)
                    instance = new GameObject("ItemManager").AddComponent<ItemMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    public void Initialize()
    {
        Array.Sort(items, (a, b) => (a.ID < b.ID) ? -1 : 1);
        for(int i=0;i<dropTables.Length;i++)
            Array.Sort(dropTables[i].DropDatas, (a, b) => (a.iD < b.iD) ? -1 : 1);
    }

    public ItemData GetItem(int key) { return items[key]; }

    public List<DropItem> CheckDropTableItem(int key)
    {
        DropItemData[] dropItemData = dropTables[key].DropDatas;
        List<DropItem> dropItems = new List<DropItem>();

        for (int i=0;i<dropItemData.Length;i++)
        {
            if(dropItemData[i].percent >= UnityEngine.Random.Range(0f,100f))
            {
                DropItem dropItem;
                dropItem.id = dropItemData[i].iD;
                dropItem.amount = UnityEngine.Random.Range(dropItemData[i].amount.x, dropItemData[i].amount.y + 1);
                dropItems.Add(dropItem);
            }
        }

        return dropItems;
    }

    public int CheckDropTableGold(int key)
    {
        int gold = UnityEngine.Random.Range(dropTables[key].gold.x, dropTables[key].gold.y + 1);
        return gold;
    }
}
