using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] CharacterSelectSlot slot;
    [SerializeField] CharacterSelectSlotLock slot_lock;
    [SerializeField] GameObject[] buttones;
    [SerializeField] TextMeshProUGUI gold_text;
    [SerializeField] CharacterSelectAnimEvent anim_event;

    int index;

    private void Awake()
    {
        anim_event.Init();
    }

    public void ClickRight() 
    {
        index++;
        buttones[1].gameObject.SetActive(true);
        if (index >= GameManager.Instance.GetCharacterSize() - 1)
            buttones[0].gameObject.SetActive(false);
        SetCharacterData();
    }

    public void ClickLeft()
    {
        index--;
        buttones[0].gameObject.SetActive(true);
        if (index <= 0)
            buttones[1].gameObject.SetActive(false);
        SetCharacterData();
    }

    void SetCharacterData()
    {
        anim_event.Hide();
        anim_event.StopSFX();
        if (GameManager.Instance.GetCharacterData(index).IsLock)
        {
            slot_lock.gameObject.SetActive(true);
            slot.gameObject.SetActive(false);
            slot_lock.SetCharacterDataLock(GameManager.Instance.GetCharacterData(index));
        } 
        else
        {
            slot_lock.gameObject.SetActive(false);
            slot.gameObject.SetActive(true);
            anim_event.SetType(GameManager.Instance.GetCharacterData(index).ID);
            slot.SetCharacterData(GameManager.Instance.GetCharacterData(index));
        }
    }

    public void UpdateGold() { gold_text.text = GameManager.Instance.Gold.ToString(); }

    private void OnEnable()
    {
        UpdateGold();
        index = 1;
        ClickLeft();
    }
}
