using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour, IAudioSubject
{
    [SerializeField] SoundData[] backgroundSoundData;
    [SerializeField] SoundData[] SFXSoundData;

    [SerializeField] AudioSource backgroundAudio;
    [SerializeField] AudioSource SFXAudio;
    [SerializeField] AudioSource MoveAudio;

    List<IAudioObserver> observers;
    int listIndex = -1;

    float masterVolume;
    float backgroundVolume;
    float SFXVolume;

    static SoundMgr instance;
    public static SoundMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SoundMgr>();
                if(!instance)
                    instance = new GameObject("SoundManager").AddComponent<SoundMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    public void StopBackgroundAudio()
    {
        backgroundAudio.Stop();
    }

    public void PlayBackgroundAudio()
    {
        backgroundAudio.Play();
    }

    public void Initialize(float masterVolume, float backgroundVolume, float SFXVolume)
    {
        this.masterVolume = masterVolume;
        this.backgroundVolume = backgroundVolume;
        this.SFXVolume = SFXVolume;

        backgroundAudio.loop = true;
        SFXAudio.loop = false;

        for (int i = 0; i < SFXSoundData.Length; i++)
        {
            if (SFXSoundData[i].SoundName == "Walk")
            {
                MoveAudio.clip = SFXSoundData[i].Clip;
                MoveAudio.loop = true;
                break;
            }
        }
        MoveAudio.pitch = 0.75f;
        observers = new();
    }

    public void PlayMoveSound()
    {
        if (!MoveAudio.isPlaying)
        {
            MoveAudio.volume = masterVolume * SFXVolume;
            MoveAudio.Play();
        }
    }

    public void Run(bool isRun)
    {
        if(isRun)
            MoveAudio.pitch = 1f;
        else
            MoveAudio.pitch = 0.75f;
    }

    public void StopMoveSound()
    {
        MoveAudio.Stop();
    }

    public void PlayBackgroundAudio(string name)
    {
        for(int i=0;i< backgroundSoundData.Length;i++)
        {
            if (backgroundSoundData[i].SoundName == name)
            {
                backgroundAudio.volume = backgroundVolume * masterVolume;
                backgroundAudio.clip = backgroundSoundData[i].Clip;
                backgroundAudio.Play();
                return;
            }  
        }
    }

    public void PlaySFXAudio(string name)
    {
        for(int i=0;i<SFXSoundData.Length;i++)
        {
            if (SFXSoundData[i].SoundName == name)
            {
                SFXAudio.volume = SFXVolume * masterVolume;
                SFXAudio.PlayOneShot(SFXSoundData[i].Clip);
                return;
            }  
        }
    }

    public void AddObserver(IAudioObserver o)
    {
        if(observers.Find(x => x == o) == null)
        {
            observers.Add(o);
            o.UpdateMasterVolume(masterVolume);
            o.UpdateSFXVolume(SFXVolume);
            listIndex++;
        }
    }

    public void RemoveObserver(IAudioObserver o)
    {
        if (observers.Find(x => x == o) != null)
        {
            observers.Remove(o);
            listIndex--;
        }
    }

    public void UpdateMasterVolume(float masterVolume)
    {
        this.masterVolume = masterVolume;
        backgroundAudio.volume = this.masterVolume * backgroundVolume;
        SFXAudio.volume = this.masterVolume * SFXVolume;
        for (int i = 0; i <= listIndex; i++)
            observers[i].UpdateMasterVolume(masterVolume);
    }

    public void UpdateSFXVolume(float SFXVolume)
    {
        this.SFXVolume = SFXVolume;
        SFXAudio.volume = this.masterVolume * this.SFXVolume;
        for (int i = 0; i <= listIndex; i++)
            observers[i].UpdateMasterVolume(SFXVolume);
    }

    public void UpdateBackgroundVolume(float backgroundVolume)
    {
        this.backgroundVolume = backgroundVolume;
        backgroundAudio.volume = masterVolume * this.backgroundVolume;
    }

    public AudioClip GetAudioClip(string name)
    {
        for (int i = 0; i < SFXSoundData.Length; i++)
        {
            if (SFXSoundData[i].SoundName == name)
                return SFXSoundData[i].Clip;
        }

        return null;
    }
}
