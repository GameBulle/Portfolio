using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Sound Data", menuName = "ScriptableObject/SFX/Player Sound Data",order = 1)]
public class PlayerSoundData : ScriptableObject
{
    [SerializeField] SoundData move;

    public SoundData Move => move;

    public PlayerSoundData()
    {
        move = new SoundData(1.0f);
    }
}
