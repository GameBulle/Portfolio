using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Sound Data", menuName = "ScriptableObject/SFX/Monster Sound Data", order = 4)]
public class MonsterSoundData : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] SoundData attack;
    [SerializeField] SoundData die;
    [SerializeField] SoundData spawn;

    public int Id => id;
    public SoundData Attack => attack;
    public SoundData Die => die;
    public SoundData Spawn => spawn;

    public MonsterSoundData()
    {
        attack = new SoundData(1.0f);
        die = new SoundData(1.0f);
        spawn = new SoundData(1.0f);
    }
}
