using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Data", menuName = "ScriptableObject/Sound/Sound", order = 0)]
public class SoundData : ScriptableObject
{
    [SerializeField] string soundName;
    [SerializeField] AudioClip clip;

    public string SoundName => soundName;
    public AudioClip Clip => clip;
}
