using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow Sound Data",menuName = "ScriptableObject/SFX/Bow Sound Data",order =2)]
public class BowSoundData : ScriptableObject
{
    [SerializeField] SoundData pull;
    [SerializeField] SoundData plusPull;
    [SerializeField] SoundData shot;

    public SoundData Pull => pull;
    public SoundData Plus => plusPull;
    public SoundData Shot => shot;

    public BowSoundData()
    {
        pull = new SoundData(1.0f);
        plusPull = new SoundData(1.0f);
        shot = new SoundData(1.0f);
    }
}
