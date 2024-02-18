using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interface Sound Data", menuName = "ScriptableObject/SFX/Interface Sound Data", order = 6)]
public class InterfaceSoundData : ScriptableObject
{
    [SerializeField] SoundData paperFlip;
    [SerializeField] SoundData waveClear;
    [SerializeField] SoundData waveStart;
    [SerializeField] SoundData victory;
    [SerializeField] SoundData gameOver;
    [SerializeField] SoundData make;
    [SerializeField] SoundData repair;
    [SerializeField] SoundData levelUP;
    [SerializeField] SoundData hi;
    [SerializeField] SoundData equip;

    public SoundData PaperFlip => paperFlip;
    public SoundData WaveClear => waveClear;
    public SoundData WaveStart => waveStart;
    public SoundData Victory => victory;
    public SoundData GameOver => gameOver;
    public SoundData Make => make;
    public SoundData Repair => repair;
    public SoundData LevelUP => levelUP;
    public SoundData Hi => hi;
    public SoundData Equip => equip;

    public InterfaceSoundData()
    {
        paperFlip = new SoundData(1.0f);
        waveClear = new SoundData(1.0f);
        waveStart = new SoundData(1.0f);
        victory = new SoundData(1.0f);
        gameOver = new SoundData(1.0f);
        make = new SoundData(1.0f);
        repair = new SoundData(1.0f);
        levelUP = new SoundData(1.0f);
        hi = new SoundData(1.0f);
        equip = new SoundData(1.0f);
    }
}
