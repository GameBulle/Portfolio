using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InterfaceMgr : MonoBehaviour
{
    static InterfaceMgr instance = null;

    public static InterfaceMgr Instance
    {
        get
        {
            if(null == instance)
            {
                instance = FindObjectOfType<InterfaceMgr>();
                if (!instance)
                    instance = new GameObject("InterfaceManager").AddComponent<InterfaceMgr>();

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("WaveClearGUI")]
    [SerializeField] WaveClearUI waveClearUI;

    [Header("PlayGUI")]
    [SerializeField] PlayUI playUI;

    [Header("MainUI")]
    [SerializeField] GameObject mainUI;

    [Header("PauseUI")]
    [SerializeField] GameObject pauseUI;

    [Header("HelpUI")]
    [SerializeField] HelpUI helpUI;

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    private void Start()
    {
        mainUI.SetActive(true);
    }

    public void Initialize()
    {
        helpUI.gameObject.SetActive(false);

        waveClearUI.Initialize();
        waveClearUI.gameObject.SetActive(false);

        playUI.Initialize();
        playUI.gameObject.SetActive(false);

        pauseUI.gameObject.SetActive(false);
    }

    public void WaveClear()
    {
        waveClearUI.gameObject.SetActive(true);
        waveClearUI.WaveClear();
    }

    public void WaveStart()
    {
        waveClearUI.gameObject.SetActive(false);

        playUI.gameObject.SetActive(true);
        playUI.WaveStart();
    }

    public void UseArrow1()
    {
        playUI.UseArrow1();
    }

    public void UseArrow2()
    {
        playUI.UseArrow2();
    }

    public void UseNormalArrow()
    {
        playUI.UseNormalArrow();
    }

    public void UpdateArrowInfo(int arrowIndex)
    {
        if (arrowIndex == 0)
            playUI.GetArrow1Name();
        else if (arrowIndex == 1)
            playUI.GetArrow2Name();
    }

    public void MonsterCount()
    {
        playUI.MonsterCount();
    }

    public void GameClear()
    {
        SoundMgr.Instance.VictorySoundPlay();
        waveClearUI.GameClear();
    }

    public void GameOver()
    {
        waveClearUI.gameObject.SetActive(true);
        waveClearUI.GameOver();

        SoundMgr.Instance.GameOverSoundPlay();
    }

    public void HpSlider()
    {
        playUI.HpSlider();
    }

    public void SetHpSlider()
    {
        playUI.SetHpSlider();
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void MainTitle()
    {
        mainUI.SetActive(true);

        playUI.gameObject.SetActive(false);
        waveClearUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);
    }

    public void ClickStart()
    {
        mainUI.SetActive(false);
        helpUI.gameObject.SetActive(true);
        helpUI.Initialize();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        Time.timeScale = 0;
        GameMgr.Instance.IsPause = true;
        pauseUI.gameObject.SetActive(true);
    }

    public void ClickContinue()
    {
        Time.timeScale = 1;
        GameMgr.Instance.IsPause = false;
        pauseUI.gameObject.SetActive(false);
    }
}
