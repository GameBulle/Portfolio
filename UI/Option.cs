using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] Slider masterVolume_slider;
    [SerializeField] Slider backgroundVolume_slider;
    [SerializeField] Slider SFXVolume_slider;

    [SerializeField] TextMeshProUGUI masterVolume_text;
    [SerializeField] TextMeshProUGUI backgroundVolume_text;
    [SerializeField] TextMeshProUGUI SFXVolume_text;

    [SerializeField] GameObject[] button_checks;

    float masterVolume;
    float backgroundVolume;
    float SFXVolume;
    int FPS;

    private void Awake()
    {
        masterVolume_slider.maxValue = 1f;
        backgroundVolume_slider.maxValue = 1f;
        SFXVolume_slider.maxValue = 1f;
    }

    private void Update()
    {
        if(masterVolume != masterVolume_slider.value)
        {
            masterVolume = masterVolume_slider.value;
            masterVolume_text.text = ((int)(masterVolume * 100)).ToString();
            AudioManager.Instance.UpdateMasterVolume(this.masterVolume);
            AudioManager.Instance.PlaySFX("OptionSlider");
        }

        if(backgroundVolume != backgroundVolume_slider.value)
        {
            backgroundVolume = backgroundVolume_slider.value;
            backgroundVolume_text.text = ((int)(backgroundVolume * 100)).ToString();
            AudioManager.Instance.UpdateBackgroundVolume(this.backgroundVolume);
            AudioManager.Instance.PlaySFX("OptionSlider");
        }

        if(SFXVolume != SFXVolume_slider.value)
        {
            SFXVolume = SFXVolume_slider.value;
            SFXVolume_text.text = ((int)(SFXVolume * 100)).ToString();
            AudioManager.Instance.UpdateSFXVolume(this.SFXVolume);
            AudioManager.Instance.PlaySFX("OptionSlider");
        }
    }

    public void Initialize(OptionData data)
    {
        this.masterVolume = data.masterVolume;
        this.backgroundVolume = data.backgroundVolume;
        this.SFXVolume = data.SFXVolume;
        this.FPS = data.FPS;

        masterVolume_slider.value = this.masterVolume;
        masterVolume_text.text = ((int)(masterVolume * 100)).ToString();
        backgroundVolume_slider.value = this.backgroundVolume;
        backgroundVolume_text.text = ((int)(backgroundVolume * 100)).ToString();
        SFXVolume_slider.value = this.SFXVolume;
        SFXVolume_text.text = ((int)(SFXVolume * 100)).ToString();

        switch(this.FPS)
        {
            case 30:
                ClickFPS(true);
                break;
            case 60:
                ClickFPS(false);
                break;
        }

        AudioManager.Instance.UpdateMasterVolume(this.masterVolume);
        AudioManager.Instance.UpdateBackgroundVolume(this.backgroundVolume);
        AudioManager.Instance.UpdateSFXVolume(this.SFXVolume);
        GameManager.Instance.SetFPS(this.FPS);
    }

    public void ClickFPS(bool clickLeft)
    {
        if (clickLeft)
        {
            button_checks[0].gameObject.SetActive(clickLeft);
            button_checks[1].gameObject.SetActive(!clickLeft);
            FPS = 15;
        }
        else
        {
            button_checks[0].gameObject.SetActive(clickLeft);
            button_checks[1].gameObject.SetActive(!clickLeft);
            FPS = 60;
        }
        GameManager.Instance.SetFPS(FPS);
    }
}
