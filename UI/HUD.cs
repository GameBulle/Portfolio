using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Slider exp_slider;
    [SerializeField] Slider mp_slider;
    [SerializeField] Slider hp_slider;
    [SerializeField] TextMeshProUGUI time_text;
    [SerializeField] TextMeshProUGUI level_text;
    [SerializeField] Image gold_icon;
    [SerializeField] TextMeshProUGUI gold_text;
    [SerializeField] JoyStick joyStick;

    RectTransform hp_rect;

    private void Awake()
    {
        hp_rect = hp_slider.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        hp_rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.Player.transform.position + Vector3.down * 1f);
    }

    private void LateUpdate()
    {
        UpdateTime();
        UpdateMP();
    }

    public void Initialize(float next, float hp, float mp)
    {
        hp_slider.maxValue = hp;
        hp_slider.value = hp;
        exp_slider.maxValue = next;
        exp_slider.value = 0f;
        mp_slider.maxValue = mp;
        mp_slider.value = 0f;
        level_text.text = "LV : 1";
        gold_text.text = ": 0";
    }

    void UpdateTime()
    {
        float game_time = GameManager.Instance.GameTime;
        int min = Mathf.FloorToInt(game_time / 60);
        int sec = Mathf.FloorToInt(game_time % 60);
        time_text.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }

    void UpdateMP() { mp_slider.value = GameManager.Instance.CurrMP; }
    public void UpdateGold(int gold)
    {
        StringBuilder sb = new();
        sb.Append(": ");
        sb.Append(gold.ToString());
        gold_text.text = sb.ToString();
    }

    public void UpdateHP(float hp, float max)
    {
        hp_slider.maxValue = max;
        hp_slider.value = hp;
    }

    public void UpdateEXP(float curr, float next)
    {
        exp_slider.maxValue = next;
        exp_slider.value = curr;
    }

    public void UpdateLevel(int level)
    {
        StringBuilder sb = new();
        sb.Append("LV : ");
        sb.Append(level.ToString());
        level_text.text = sb.ToString();
        joyStick.Hide();
    }
}
