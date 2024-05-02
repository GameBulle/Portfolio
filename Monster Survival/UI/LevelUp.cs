using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [SerializeField] LevelUpSlot[] slots;
    [SerializeField] TextMeshProUGUI redraw_text;
    int index = 0;
    int redraw;

    private void Awake()
    {
        gameObject.SetActive(false);
        redraw = 2;
        UpdateReDrawButton();
    }

    public void SetSkill(WeaponData data, int level)
    {
        gameObject.SetActive(true);
        slots[index].SetSkill(data, level);
        index++;
        if (index > 2)
            index = 0;
    }

    public void SetSkill(PassiveData data, int level)
    {
        gameObject.SetActive(true);
        slots[index].SetSkill(data, level);
        index++;
        if(index > 2) 
            index = 0;
    }

    public void OnClickReDraw()
    {
        if (redraw <= 0)
            return;
        redraw--;
        UpdateReDrawButton();
        GameManager.Instance.ReDraw();
    }

    void UpdateReDrawButton()
    {
        StringBuilder sb = new();
        sb.Append("스킬 다시 뽑기(");
        sb.Append(redraw.ToString());
        sb.Append(")");
        redraw_text.text = sb.ToString();
    }
}
