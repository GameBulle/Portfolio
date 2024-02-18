using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMgr : MonoBehaviour
{
    PlayerInput input;
    [Header("Function")]
    [SerializeField] Player player;
    [SerializeField] DefenceLine defenceLine;
    [SerializeField] MonsterMgr monsterMgr;
    [SerializeField] NPCMgr npcMgr;

    [Header("Clear Wave")]
    [SerializeField] int clearWave = 0;

    [Header("Resolution")]
    [SerializeField] int width = 0;
    [SerializeField] int height = 0;

    [Header("Frame")]
    [SerializeField] int fps = 0;

    static GameMgr instance = null;
    public static GameMgr Instance
    {
        get
        {
            if(null == instance)
            {
                instance = FindObjectOfType<GameMgr>();
                if (!instance)
                    instance = new GameObject("GameManager").AddComponent<GameMgr>();

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    public int Wave { get; set; }
    int monsterCount;
    public int WaveMonster { get; private set; }

    public bool IsPause;

    public int MonsterCount { get { return monsterCount; }
        set
        {
            monsterCount = value;
            InterfaceMgr.Instance.MonsterCount();

            if(0 >= monsterCount) 
                StartCoroutine(OnWaveClear());
        }
    }


    private void Awake()
    {
        Screen.SetResolution(width, height, true);
        Application.targetFrameRate = fps;

        if (this != Instance)
            Destroy(gameObject);
    }

    public void Initialize()
    {
        Wave = 0;
        monsterCount = 0;
        WaveMonster = 0;
        IsPause = false;
        Time.timeScale = 1;

        if (TryGetComponent(out input))
        {
            input.SwitchCurrentActionMap("Player");
            input.currentActionMap.Disable();

            input.actions["Move"].performed += player.MoveAction;
            input.actions["Move"].canceled += player.StopAction;

            input.actions["Shot"].performed += player.ChargeAction;
            input.actions["Shot"].canceled += player.ShotAction;

            input.actions["ChangeArrow1"].started += player.ChangeArrow1Action;
            input.actions["ChangeArrow2"].started += player.ChangeArrow2Action;

            input.actions["ESC"].started += InterfaceMgr.Instance.Pause;
        }

        monsterMgr.Initialize();
        player.PlayerInitialize();
        defenceLine.Initialize();
        ArrowSpawner.Instance.Initialize();
        InterfaceMgr.Instance.Initialize();
        npcMgr.Initialize();

        WaveStart();
    }

    void WaveStart()
    {
        Wave++;
        input.currentActionMap.Enable();

        defenceLine.WaveStart();
        WaveMonster = monsterMgr.WaveStart();
        monsterCount = WaveMonster;
        npcMgr.WaveStart();
        ItemMgr.Instance.WaveStart();
        InterfaceMgr.Instance.WaveStart();
        player.WaveStart();

        SoundMgr.Instance.WaveStartSoundPlay();
        SoundMgr.Instance.BackgroundSoundPlay();
    }

    IEnumerator OnWaveClear()
    {
        yield return new WaitForSeconds(2.0f);

        input.currentActionMap.Disable();

        defenceLine.WaveClear();
        player.WaveClear();
        ItemMgr.Instance.WaveClear();
        InterfaceMgr.Instance.WaveClear();
        ArrowSpawner.Instance.WaveClear();
        monsterMgr.WaveClear();
        npcMgr.WaveClear();

        if (CheckVictory())
            InterfaceMgr.Instance.GameClear();

        SoundMgr.Instance.BackgroundSoundStop();
    }

    bool CheckVictory()
    {
        if (Wave == clearWave)
            return true;

        return false;
    }

    public void GameOver()
    {
        player.GameOver();
        npcMgr.GameOver();
        monsterMgr.GameOver();
        if(!IsPause)
            InterfaceMgr.Instance.GameOver();

        SoundMgr.Instance.BackgroundSoundStop();
    }
}
