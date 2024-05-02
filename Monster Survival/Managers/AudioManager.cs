using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (!instance)
                    instance = new GameObject("AudioManager").AddComponent<AudioManager>();
            }
            return instance;
        }
    }

    [SerializeField] AudioClip main_background_clip;
    [SerializeField] SoundData[] datas;
    [SerializeField] AudioSource background_audio;
    [SerializeField] AudioSource SFX_audio;
    [SerializeField] AudioSource character_select_audio;

    AudioHighPassFilter BGM_effect;
    float masterVolume;
    float backgroundVolume;
    float sfxVolume;

    public float MasterVolume => masterVolume;
    public float BackgroundVolume => backgroundVolume;
    public float SFXVolume => sfxVolume;

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
        background_audio.loop = true;
        SFX_audio.loop = false;
        character_select_audio.loop = false;
        BGM_effect = Camera.main.GetComponent<AudioHighPassFilter>();
        SFX_audio.bypassListenerEffects = true;
    }

    public void Initialize(float masterVolume, float backgroundVolume, float SFXVolume)
    {
        this.masterVolume = masterVolume;
        this.backgroundVolume = backgroundVolume;
        this.sfxVolume = SFXVolume;

        background_audio.volume = this.backgroundVolume * this.masterVolume;
        SFX_audio.volume = sfxVolume * this.masterVolume;
        character_select_audio.volume = sfxVolume * this.masterVolume;
        PlayBackground();
    }

    public void BGMEffect(bool isOn)
    {
        BGM_effect.enabled = isOn;
    }

    public void PlayBackground()
    {
        background_audio.clip = main_background_clip;
        background_audio.Play();
    }

    public void StopBackground()
    {
        background_audio.Stop();
    }

    public void PlayBackground(AudioClip clip)
    {
        background_audio.clip = clip;
        background_audio.Play();
    }

    public void PlaySFX(string name)
    {
        int index = Array.FindIndex(datas, x => x.SoundName == name);
        SFX_audio.PlayOneShot(datas[index].Clip);
    }

    public void PlayCharacterSelectSFX(string name)
    {
        int index = Array.FindIndex(datas, x => x.SoundName == name);
        character_select_audio.clip = datas[index].Clip;
        character_select_audio.Play();
    }

    public void StopCharacterSelectSFX() { character_select_audio.Stop(); }

    public void UpdateMasterVolume(float masterVolume)
    {
        this.masterVolume = masterVolume;
        background_audio.volume = backgroundVolume * this.masterVolume;
        SFX_audio.volume = sfxVolume * this.masterVolume;
    }

    public void UpdateBackgroundVolume(float backgroundVolume)
    {
        this.backgroundVolume = backgroundVolume;
        background_audio.volume = this.backgroundVolume * masterVolume;
    }

    public void UpdateSFXVolume(float SFXVolume)
    {
        this.sfxVolume = SFXVolume;
        SFX_audio.volume = sfxVolume * masterVolume;
        character_select_audio.volume = sfxVolume * masterVolume;
    }
}
