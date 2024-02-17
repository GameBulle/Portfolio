using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : BillBoard
{
    [Header("UI")]
    [SerializeField] Slider warningSlider;
    [SerializeField] GameObject warningImage;
    [SerializeField] Slider hpSlider;

    private void Update()
    {
        if (Vector3.Distance(transform.position, GameMgr.Instance.mainCam.transform.position) <= 150)
            hpSlider.gameObject.SetActive(true);
        else
            hpSlider.gameObject.SetActive(false);
    }



    public void Initialize(float warningSliderMax, float hpSliderMax)
    {
        warningSlider.maxValue = warningSliderMax;
        hpSlider.maxValue = hpSliderMax;
        hpSlider.value = hpSlider.maxValue;
        warningSlider.gameObject.SetActive(false);
        warningImage.gameObject.SetActive(false);
        hpSlider.gameObject.SetActive(false);
    }

    public void WarningSliderOn()
    {
        warningSlider.gameObject.SetActive(true);
    }

    public void WarningSliderOff()
    {
        warningSlider.gameObject.SetActive(false);
    }

    public void WarningImageOn()
    {
        warningImage.gameObject.SetActive(true);
    }

    public void WarningImageOff()
    {
        warningImage.gameObject.SetActive(false);
    }

    public void WarningSliderUpdate(float value, bool damaged = false)
    {
        warningSlider.value = value;
        if (warningSlider.value > 0 && !damaged)
            warningSlider.gameObject.SetActive(true);
        else
            warningSlider.gameObject.SetActive(false);
    }

    public void UpdateHP(float hp)
    {
        hpSlider.value = hp;
    }
}
