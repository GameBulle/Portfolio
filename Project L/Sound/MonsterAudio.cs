using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudio : MonoBehaviour, IAudioObserver
{
    [SerializeField] AudioSource monsterAudio;

    IAudioObserver observer;
    float masterVolume;
    float SFXVolume;

    public void Initialize()
    {
        observer = GetComponent<IAudioObserver>();
        SoundMgr.Instance.AddObserver(observer);
    }

    public void PlaySFX(string ClipName)
    {
        AudioClip clip = SoundMgr.Instance.GetAudioClip(ClipName);
        if (clip != null)
        {
            monsterAudio.volume = masterVolume * SFXVolume;
            monsterAudio.PlayOneShot(clip);
        }
        else
            Debug.Log(ClipName + " 이름을 가진 Audio가 없습니다.");
    }

    public void UpdateMasterVolume(float masterVolume)
    {
        this.masterVolume = masterVolume;
        monsterAudio.volume = this.masterVolume * SFXVolume;
    }

    public void UpdateSFXVolume(float SFXVolume)
    {
        this.SFXVolume = SFXVolume;
        monsterAudio.volume = masterVolume * this.SFXVolume;
    }

    private void OnDisable()
    {
        SoundMgr.Instance.RemoveObserver(observer);
    }
}
