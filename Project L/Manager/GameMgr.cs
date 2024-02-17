using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMgr : MonoBehaviour
{
    //UnityEngine.InputSystem.PlayerInput input;
    [SerializeField] UnityEngine.InputSystem.PlayerInput input;
    [SerializeField] CopyPosition minimapCamera;
    [SerializeField] Player playerPrefab;

    Coroutine coroutine;
    Transform mainCameraPos;
    Player player;
    public bool isShop;
    float sensivity;

    public bool IsTalk { get; set; }
    public bool IsAction { get; set; }
    public Camera mainCam { get; private set; }
    public int OpenUICount { get; set; }
    public FullScreenMode screenMode { get; private set; }

    static GameMgr instance = null;
    public static GameMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameMgr>();
                if (!instance)
                    instance = new GameObject("GameManager").AddComponent<GameMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);

        isShop = false;
        IsTalk = false;
        IsAction = false;
        
    }

    public void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }

    public void SetScreenMode(FullScreenMode mode)
    {
        screenMode = mode;
        if (FullScreenMode.ExclusiveFullScreen == mode)
        {
            screenMode = FullScreenMode.FullScreenWindow;
        }

        Screen.fullScreenMode = screenMode;
    }

    public void SetResolution(OptionUI.RESOLUTION resolution)
    {
        int width = 0;
        int height = 0;
        switch (resolution)
        {
            case OptionUI.RESOLUTION.HD: width = 1280; height = 720; break;
            case OptionUI.RESOLUTION.FHD: width = 1920; height = 1080; break;
            case OptionUI.RESOLUTION.QHD: width = 2560; height = 1440; break;
        }
        Screen.SetResolution(width, height, screenMode);
    }

    public void SetPlayerMouseSensivity(float value)
    {
        sensivity = value;
        if (player != null)
            player.SetSensivity(sensivity);
    }

    void MakePlayer()
    {
        player = Instantiate(playerPrefab);
        mainCam = Camera.main;
        mainCameraPos = player.GetMainCameraPos();
        player.gameObject.SetActive(true);
    }

    public void Initialize(PlayerSaveData data = null)
    {
        MakePlayer();

        if (TryGetComponent(out input))
        {
            input.SwitchCurrentActionMap("Player");
            input.currentActionMap.Disable();

            input.actions["Run"].performed += player.RunAction;
            input.actions["Run"].canceled += player.RunStop;

            input.actions["MouseRotate"].performed += player.MouseDelta;
            input.actions["Evade"].started += player.EvadeAction;

            input.actions["NormalAttack"].started += player.NormalAttackAction;
            input.actions["Guard"].started += player.GuardAction;

            input.actions["LockOn"].started += player.LockOnAction;
            input.actions["LockOnMove"].started += player.LockOnMoveAction;

            input.actions["Inventory"].started += player.InventoryAction;
            input.actions["Interaction"].started += player.InteractionAction;

            input.actions["QuickSlot1"].started += player.QuickSlot1;
            input.actions["QuickSlot2"].started += player.QuickSlot2;
            input.actions["QuickSlot3"].started += player.QuickSlot3;
            input.actions["QuickSlot4"].started += player.QuickSlot4;
            input.actions["QuickSlot5"].started += player.QuickSlot5;
            input.actions["QuickSlot6"].started += player.QuickSlot6;
            input.actions["QuickSlot7"].started += player.QuickSlot7;
            input.actions["QuickSlot8"].started += player.QuickSlot8;

            input.actions["QuickSkill1"].started += player.QuickSkill1;
            input.actions["QuickSkill2"].started += player.QuickSkill2;
            input.actions["QuickSkill3"].started += player.QuickSkill3;

            input.actions["SkillUI"].started += player.OpenSkillUI;

            input.actions["PlayerInfo"].started += player.OpenPlayerInfoAction;
            input.actions["SelectInteraction"].started += player.SelectInterAction;

            input.actions["Map"].started += player.MapAction;
            input.actions["ESC"].started += player.ESCAction;

            input.currentActionMap.Enable();
        }

        OpenUICount = 0;

        ItemMgr.Instance.Initialize();
        SkillMgr.Instance.Initialize(data);
        player.Initialize(data,sensivity);
        minimapCamera.gameObject.SetActive(true);
        minimapCamera.Initialize(player.transform);
        EffectMgr.Instance.Initialize();
        MapMgr.Instance.Initialize();
        QuestMgr.Instance.Initialize(player);
        MonsterMgr.Instance.Initialize();
        ItemBoxMgr.Instance.Initialize();
    }

    public void SetPlayerPos(Vector3 pos, Vector3 angle)
    {
        player.gameObject.transform.position = pos;
        player.gameObject.transform.eulerAngles = angle;
        player.gameObject.SetActive(true);
    }

    public void ShakeCamera(float shakeTime, float shakeIntensity)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnShakeCamera(shakeTime, shakeIntensity));
    }

    IEnumerator OnShakeCamera(float shakeTime, float shakeIntensity)
    {
        if(mainCam == null)
            mainCam = Camera.main;
        while(shakeTime > 0f)
        {
            mainCam.transform.position = mainCameraPos.position + Random.insideUnitSphere * shakeIntensity;
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        mainCam.transform.position = mainCameraPos.position;
    }

    public void ActivePlayer()
    {
        player.gameObject.SetActive(true);
    }

    public PlayerSaveData AutoSave()
    {
        return player.MakeSaveData();
    }

    public void DestroyPlayer()
    {
        Player player = FindObjectOfType<Player>();
        Destroy(player.gameObject);
        input.currentActionMap.Disable();
        input.actions["Run"].performed -= player.RunAction;
        input.actions["Run"].canceled -= player.RunStop;

        input.actions["MouseRotate"].performed -= player.MouseDelta;
        input.actions["Evade"].started -= player.EvadeAction;

        input.actions["NormalAttack"].started -= player.NormalAttackAction;
        input.actions["Guard"].started -= player.GuardAction;

        input.actions["LockOn"].started -= player.LockOnAction;
        input.actions["LockOnMove"].started -= player.LockOnMoveAction;

        input.actions["Inventory"].started -= player.InventoryAction;
        input.actions["Interaction"].started -= player.InteractionAction;

        input.actions["QuickSlot1"].started -= player.QuickSlot1;
        input.actions["QuickSlot2"].started -= player.QuickSlot2;
        input.actions["QuickSlot3"].started -= player.QuickSlot3;
        input.actions["QuickSlot4"].started -= player.QuickSlot4;
        input.actions["QuickSlot5"].started -= player.QuickSlot5;
        input.actions["QuickSlot6"].started -= player.QuickSlot6;
        input.actions["QuickSlot7"].started -= player.QuickSlot7;
        input.actions["QuickSlot8"].started -= player.QuickSlot8;

        input.actions["QuickSkill1"].started -= player.QuickSkill1;
        input.actions["QuickSkill2"].started -= player.QuickSkill2;
        input.actions["QuickSkill3"].started -= player.QuickSkill3;

        input.actions["SkillUI"].started -= player.OpenSkillUI;

        input.actions["PlayerInfo"].started -= player.OpenPlayerInfoAction;
        input.actions["SelectInteraction"].started -= player.SelectInterAction;

        input.actions["Map"].started -= player.MapAction;
        input.actions["ESC"].started -= player.ESCAction;

        minimapCamera.gameObject.SetActive(false);
    }
}
