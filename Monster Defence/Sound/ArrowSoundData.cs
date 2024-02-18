using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrow Sound Data", menuName = "ScriptableObject/SFX/Arrow Sound Data", order = 3)]
public class ArrowSoundData : ScriptableObject
{
    [SerializeField] SoundData hit;

    public SoundData Hit => hit;

    public ArrowSoundData()
    {
        hit = new SoundData(1.0f);
    }
}
