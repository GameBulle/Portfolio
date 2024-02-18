using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect Sound Data", menuName = "ScriptableObject/SFX/Effect Sound Data", order = 7)]
public class EffectSoundData : ScriptableObject
{
    [SerializeField] SoundData smoke;

    public SoundData Smoke => smoke;

    public EffectSoundData()
    {
        smoke = new SoundData(1.0f);
    }
}
