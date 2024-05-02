using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Object/SoundData")]
public class SoundData : ScriptableObject
{
    [SerializeField] string sound_name;
    [SerializeField] AudioClip clip;

    public string SoundName => sound_name;
    public AudioClip Clip => clip;
}
