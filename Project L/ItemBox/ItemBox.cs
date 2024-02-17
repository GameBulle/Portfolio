using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemBox : MonoBehaviour,IInteractionable, IPoolingObject
{
    protected List<DropItem> dropItem = new List<DropItem>();
    protected List<DropItem> selectItem = new List<DropItem>();
    public event System.Action<ItemBox> OnDestroyEvent = null;

    public string InteractionName { get; set; }
    public Sprite InteractionIcon { get; set; }
    public int Gold { get; protected set; }
    protected int type;

    public virtual void InitTreasureItemBox(TreasureBoxData data)
    {
        for (int i = 0; i < data.Items.Length; i++)
            dropItem.Add(data.Items[i]);

        Gold = data.Gold;
        type = 0;
        InteractionName = "보물 상자";
        InteractionIcon = Resources.Load<Sprite>("Icon/Treasure Box");

        gameObject.SetActive(true);
    }

    public void InitDropItemBox(int dropTableKey, string monsterName)
    {
        StringBuilder sb = new();
        dropItem = ItemMgr.Instance.CheckDropTableItem(dropTableKey);
        Gold = ItemMgr.Instance.CheckDropTableGold(dropTableKey);
        type = 1;

        sb.Append(monsterName);
        sb.Append(" 전리품");
        InteractionName = sb.ToString();
        InteractionIcon = Resources.Load<Sprite>("Icon/Treasure Box");

        gameObject.SetActive(true);
        StartCoroutine(OnTest());
    }

    IEnumerator OnTest()
    {
        yield return new WaitForSeconds(30f);

        InterfaceMgr.Instance.DropItemBoxUIExit();
        RemoveInteractionToList();
        InterfaceMgr.Instance.InteractionUIExit();
        gameObject.SetActive(false);
    }

    public void ClickGet(List<int> select)
    {
        for (int i = 0, j = 0; j < select.Count; i++, j++)
        {
            selectItem.Add(dropItem[(select[i]) - i]);
            dropItem.RemoveAt((select[i] - i));
        }
    }

    public void SetPosition(Vector3 pos)
    {
        if(type == 1)
            this.transform.position = pos + (Vector3.up * this.transform.localScale.y * 0.5f);
        else
            this.transform.position = pos;
    }

    public void Interaction(Player player)
    {
        InterfaceMgr.Instance.LinkDropItemBox(this);
    }

    List<DropItem> GetItem()
    {
        List<DropItem> temp = new List<DropItem>();
        temp = selectItem.ToList();
        selectItem.Clear();
        AfterGet();

        return temp;
    }

    public void InteractionGetItem(Player player)
    {
        player.GetItems(this.GetItem());
        player.PlusGold(Gold);
        Gold = 0;
    }

    public void AddInteractionToList()
    {
        if (dropItem.Count > 0)
            InterfaceMgr.Instance.AddInteraction(this.gameObject);
    }

    public void RemoveInteractionToList()
    {
        InterfaceMgr.Instance.RemoveInteraction(this.gameObject);
    }

    public void SetAngle(Vector3 angle)
    {
        gameObject.transform.eulerAngles = angle;
    }

    private void OnDisable()
    {
        OnDestroyEvent?.Invoke(this);
        OnDestroyEvent = null;
    }

    protected virtual void AfterGet() { }

    public List<DropItem> GetDropItem() { return dropItem; }
}
