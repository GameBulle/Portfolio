using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioObserver
{
    public void UpdateMasterVolume(float masterVolume);
    public void UpdateSFXVolume(float SFXVolume);
}
