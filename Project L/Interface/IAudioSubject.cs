using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioSubject
{
    void AddObserver(IAudioObserver o);
    void RemoveObserver(IAudioObserver o);
    void UpdateMasterVolume(float masterVolume);
    void UpdateSFXVolume(float SFXVolume);
    void UpdateBackgroundVolume(float backgroundVolume);
}
