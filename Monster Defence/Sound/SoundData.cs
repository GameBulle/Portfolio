using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volume;

    public SoundData(float volume)
    {
        audioClip = null;
        this.volume = volume;
    }
}
