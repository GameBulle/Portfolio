using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Image button_border;
    [SerializeField] Image button;

    private void FixedUpdate()
    {
        if(GameManager.Instance.IsMaxMp)
        {
            button_border.sprite = Resources.Load<Sprite>("Sprite/Button/Border/Border Element");
            button.sprite = Resources.Load<Sprite>("Sprite/Button/Green/Hover");
        }
    }

    public void OnClick()
    {
        button_border.sprite = Resources.Load<Sprite>("Sprite/Button/Disabled/Disabled Border Element");
        button.sprite = Resources.Load<Sprite>("Sprite/Button/Disabled/Disabled Button");
    }
}
