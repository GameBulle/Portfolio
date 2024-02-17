using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class InputUI : MoveableUI
{
    [Header("InputField")]
    [SerializeField] TMP_InputField input;

    RectTransform rect;
    Transform parentTr;
    int limitAmount;
    int slotIndex;
    MakeSlot.SlotType slotType;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        parentTr = transform.parent;
    }

    private void Update()
    {
        if(input.text.Length > 0)
        {
            int amount = int.Parse(input.text);
            if (amount > limitAmount)
                input.text = limitAmount.ToString();
            else if (amount < 0)
                input.text = "0";
        }
    }

    public void Initialize(Vector3 mousePos, int itemAmount, int slotIndex, MakeSlot.SlotType slotType)
    {
        gameObject.SetActive(true);
        input.text = "0";
        rect.position = mousePos;
        limitAmount = itemAmount;
        this.slotIndex = slotIndex;
        this.slotType = slotType;
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory UI").transform);
    }

    public void YesButton()
    {
        Debug.Log("YesButton");
        InterfaceMgr.Instance.InputAmount(slotType, slotIndex, int.Parse(input.text));
        RollbackTransform();
        gameObject.SetActive(false);
    }

    public void NoButton()
    {
        RollbackTransform();
        gameObject.SetActive(false);
    }

    void RollbackTransform()
    {
        gameObject.transform.SetParent(parentTr);
        gameObject.transform.localPosition = Vector3.zero;
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
