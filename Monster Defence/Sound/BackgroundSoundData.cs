using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Background Sound Data", menuName = "ScriptableObject/SFX/Background Sound Data", order = 5)]
public class BackgroundSoundData : ScriptableObject
{
    [SerializeField] SoundData wavePlay;

    public SoundData WavePlay => wavePlay;

    public BackgroundSoundData()
    {
        wavePlay = new SoundData(1.0f);
    }
}
