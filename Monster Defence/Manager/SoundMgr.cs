using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    static SoundMgr instance = null;

    public static SoundMgr Instance
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType<SoundMgr>();
                if (!instance)
                    instance = new GameObject("SoundMgr").AddComponent<SoundMgr>();

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    [Header("Interface Sound Data")]
    [SerializeField] InterfaceSoundData interfaceSound;

    [Header("Arrow Sound Data")]
    [SerializeField] ArrowSoundData arrowSound;

    [Header("Background Sound Data")]
    [SerializeField] BackgroundSoundData backgroundSound;
    AudioSource backgroundAudio;

    [Header("Monster Sound Data")]
    [SerializeField] MonsterSoundData[] monsterSound;

    [Header("Bow Sound Data")]
    [SerializeField] BowSoundData bowSound;

    [Header("Player Sound Data")]
    [SerializeField] PlayerSoundData playerSound;
    AudioSource playerAudio;

    [Header("Effect Sound Data")]
    [SerializeField] EffectSoundData effectSound;

    AudioSource SFXAudio;

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);

        backgroundAudio = gameObject.AddComponent<AudioSource>();
        backgroundAudio.loop = true;
        playerAudio = gameObject.AddComponent<AudioSource>();
        playerAudio.loop = true;
        SFXAudio = gameObject.AddComponent<AudioSource>();


        Array.Sort(monsterSound, (a, b) => (a.Id < b.Id) ? -1 : 1);
    }

    // Player Sound
    public void MoveSoundPlay()
    {
        playerAudio.PlayOneShot(playerSound.Move.audioClip, playerSound.Move.volume);
    }

    public void MoveSoundStop()
    {
        playerAudio.Stop();
    }
    // Player Sound End

    // Background Sound
    public void BackgroundSoundPlay()
    {
        backgroundAudio.PlayOneShot(backgroundSound.WavePlay.audioClip, backgroundSound.WavePlay.volume);
    }

    public void BackgroundSoundStop()
    {
        backgroundAudio.Stop();
    }
    // Background Sound End

    // Arrow Sound
    public void HitSoundPlay()
    {
        SFXAudio.PlayOneShot(arrowSound.Hit.audioClip, arrowSound.Hit.volume);
    }
    // Arrow Sound End

    // Bow Sound
    public void PullSoundPlay()
    {
        SFXAudio.PlayOneShot(bowSound.Pull.audioClip, bowSound.Pull.volume);
    }

    public void PlusPullSoundPlay()
    {
        SFXAudio.PlayOneShot(bowSound.Plus.audioClip, bowSound.Plus.volume);
    }

    public void ShotSoundPlay()
    {
        SFXAudio.PlayOneShot(bowSound.Shot.audioClip, bowSound.Shot.volume);
    }
    // Bow Sound End

    // Effect Sound
    public void EffectSoundPlay()
    {
        SFXAudio.PlayOneShot(effectSound.Smoke.audioClip, effectSound.Smoke.volume);
    }
    // Effect Sound End

    // Interface Sound
    public void PaperFlipSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.PaperFlip.audioClip, interfaceSound.PaperFlip.volume);
    }

    public void WaveClearSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.WaveClear.audioClip, interfaceSound.WaveClear.volume);
    }

    public void WaveStartSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.WaveStart.audioClip, interfaceSound.WaveStart.volume);
    }

    public void VictorySoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.Victory.audioClip, interfaceSound.Victory.volume);
    }

    public void GameOverSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.GameOver.audioClip, interfaceSound.GameOver.volume);
    }

    public void MakeSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.Make.audioClip, interfaceSound.Make.volume);
    }

    public void RepairSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.Repair.audioClip, interfaceSound.Repair.volume);
    }

    public void LevelUpSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.LevelUP.audioClip, interfaceSound.LevelUP.volume);
    }

    public void HiSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.Hi.audioClip, interfaceSound.Hi.volume);
    }

    public void EquipSoundPlay()
    {
        SFXAudio.PlayOneShot(interfaceSound.Equip.audioClip, interfaceSound.Equip.volume);
    }
    // Interface Sound End

    // Monster Sound
    public void SpawnSoundPlay(int id)
    {
        SFXAudio.PlayOneShot(monsterSound[id].Spawn.audioClip, monsterSound[id].Spawn.volume);
    }

    public void AttackSoundPlay(int id)
    {
        SFXAudio.PlayOneShot(monsterSound[id].Attack.audioClip, monsterSound[id].Attack.volume);
    }

    public void DieSoundPlay(int id)
    {
        SFXAudio.PlayOneShot(monsterSound[id].Die.audioClip, monsterSound[id].Die.volume);
    }
    // Monster Sound End
}
