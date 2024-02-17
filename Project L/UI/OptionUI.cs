using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class OptionUI : MonoBehaviour
{
    [Header("Tap")]
    [SerializeField] GameObject resolutionArea;
    [SerializeField] GameObject mouseSeneivityArea;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI resolutionText;
    [SerializeField] TextMeshProUGUI windowText;
    [SerializeField] TextMeshProUGUI fpsText;

    [Header("Select")]
    [SerializeField] GameObject selectResolution;
    [SerializeField] GameObject selectWindow;
    [SerializeField] GameObject selectFPS;

    [Header("Mouse Sensivity")]
    [SerializeField] Slider mouseSensivity;
    float sensivity;

    [Header("Audio Option")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider backgroundVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    [SerializeField] TextMeshProUGUI masterVolumeText;
    [SerializeField] TextMeshProUGUI backgroundVolumeText;
    [SerializeField] TextMeshProUGUI SFXVolumeText;

    OptionSaveData data;

    int FPS;
    FullScreenMode screenMode;
    RESOLUTION resolution;

    float masterVolume;
    float backgroundVolume;
    float SFXVolume;

    public string Path { get; private set; }

    public enum RESOLUTION { HD, FHD ,QHD}

    private void Awake()
    {
        StringBuilder sb = new();
        sb.Append(Application.persistentDataPath);
        sb.Append("/");
        sb.Append("Option");
        Path = sb.ToString();
    }

    private void Update()
    {
        if(sensivity != mouseSensivity.value)
        {
            sensivity = mouseSensivity.value;
            GameMgr.Instance.SetPlayerMouseSensivity(sensivity);
        }

        if(masterVolume != masterVolumeSlider.value)
        {
            masterVolume = masterVolumeSlider.value;
            masterVolumeText.text = ((int)(masterVolume * 100)).ToString();
            SoundMgr.Instance.UpdateMasterVolume(masterVolume);
        }

        if(backgroundVolume != backgroundVolumeSlider.value)
        {
            backgroundVolume = backgroundVolumeSlider.value;
            backgroundVolumeText.text = ((int)(backgroundVolume * 100)).ToString();
            SoundMgr.Instance.UpdateBackgroundVolume(backgroundVolume);
        }

        if(SFXVolume != SFXVolumeSlider.value)
        {
            SFXVolume = SFXVolumeSlider.value;
            SFXVolumeText.text = ((int)(SFXVolume * 100)).ToString();
            SoundMgr.Instance.UpdateSFXVolume(SFXVolume);
        }
    }

    public void Initialize(bool isOptionSaveData)
    {
        gameObject.SetActive(true);

        if(isOptionSaveData)
        {
            LoadOptionData();
            fpsText.text = data.FPS.ToString();
            FPS = data.FPS;

            switch(data.screenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    windowText.text = "Full Screen";
                    screenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case FullScreenMode.Windowed:
                    windowText.text = "Window";
                    screenMode = FullScreenMode.Windowed;
                    break;
                case FullScreenMode.FullScreenWindow:
                    windowText.text = "Max Window";
                    screenMode = FullScreenMode.FullScreenWindow;
                    break;
            }

            switch(data.resolution)
            {
                case RESOLUTION.HD:
                    resolutionText.text = "1280 x 720";
                    resolution = RESOLUTION.HD;
                    break;
                case RESOLUTION.FHD:
                    resolutionText.text = "1920 x 1080";
                    resolution = RESOLUTION.FHD;
                    break;
                case RESOLUTION.QHD:
                    resolutionText.text = "2560 x 1440";
                    resolution = RESOLUTION.QHD;
                    break;
            }

            sensivity = data.sensivity;
            masterVolume = data.masterVolume;
            backgroundVolume = data.backgroundVolume;
            SFXVolume = data.SFXVolume;
        }
        else
        {
            fpsText.text = "60";
            FPS = 60;

            windowText.text = "Full Screen";
            screenMode = FullScreenMode.ExclusiveFullScreen;

            resolutionText.text = "1920 x 1080";
            resolution = RESOLUTION.FHD;

            mouseSensivity.maxValue = 150f;
            mouseSensivity.value = 75f;
            sensivity = mouseSensivity.value;

            masterVolume = 1f;
            backgroundVolume = 1f;
            SFXVolume = 1f;
        }

        masterVolumeSlider.maxValue = 1f;
        masterVolumeSlider.value = masterVolume;
        masterVolumeText.text = Mathf.CeilToInt(masterVolume * 100).ToString();

        backgroundVolumeSlider.maxValue = 1;
        backgroundVolumeSlider.value = backgroundVolume;
        backgroundVolumeText.text = ((int)(backgroundVolume * 100)).ToString();

        SFXVolumeSlider.maxValue = 1;
        SFXVolumeSlider.value = SFXVolume;
        SFXVolumeText.text = ((int)(SFXVolume * 100)).ToString();

        GameMgr.Instance.SetFPS(60);
        GameMgr.Instance.SetScreenMode(FullScreenMode.ExclusiveFullScreen);
        GameMgr.Instance.SetResolution(RESOLUTION.FHD);
        GameMgr.Instance.SetPlayerMouseSensivity(mouseSensivity.value);
        SoundMgr.Instance.Initialize(masterVolume, backgroundVolume, SFXVolume);

        gameObject.SetActive(false);
    }

    // Tap
    public void ClickResolutionTap()
    {
        resolutionArea.SetActive(true);
        mouseSeneivityArea.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickMouseSensivityTap()
    {
        mouseSeneivityArea.SetActive(true);
        resolutionArea.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }
    //

    // Resolution Area//
    public void ClickSelectResolution()
    {
        if(selectResolution.activeSelf == true)
            selectResolution.SetActive(false);
        else
            selectResolution.SetActive(true);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickSelectWindow()
    {
        if(selectWindow.activeSelf == true)
            selectWindow.SetActive(false);
        else
            selectWindow.SetActive(true);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickSelectFPS()
    {
        if(selectFPS.activeSelf == true)
            selectFPS.SetActive(false);
        else
            selectFPS.SetActive(true);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }


    // Select Resolution //
    public void ClickHD()
    {
        resolutionText.text = "1280 x 720";
        GameMgr.Instance.SetResolution(RESOLUTION.HD);
        resolution = RESOLUTION.HD;
        selectResolution.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickFHD()
    {
        resolutionText.text = "1920 x 1080";
        GameMgr.Instance.SetResolution(RESOLUTION.FHD);
        resolution = RESOLUTION.FHD;
        selectResolution.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickQFD()
    {
        resolutionText.text = "2560 x 1440";
        GameMgr.Instance.SetResolution(RESOLUTION.QHD);
        resolution = RESOLUTION.QHD;
        selectResolution.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }
    ///////////////////////
    
    // Select Window //
    public void ClickFullScreen()
    {
        windowText.text = "Full Screen";
        GameMgr.Instance.SetScreenMode(FullScreenMode.ExclusiveFullScreen);
        screenMode = FullScreenMode.ExclusiveFullScreen;
        selectWindow.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickWindow()
    {
        windowText.text = "Window";
        GameMgr.Instance.SetScreenMode(FullScreenMode.Windowed);
        screenMode = FullScreenMode.Windowed;
        selectWindow.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickMaxWindow()
    {
        windowText.text = "Max Window";
        GameMgr.Instance.SetScreenMode(FullScreenMode.FullScreenWindow);
        screenMode = FullScreenMode.FullScreenWindow;
        selectWindow.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }
    //////////////////////
    
    // Select FPS //
    public void Click60FPS()
    {
        fpsText.text = "60";
        GameMgr.Instance.SetFPS(60);
        FPS = 60;
        selectFPS.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void Click30FPS()
    {
        fpsText.text = "30";
        GameMgr.Instance.SetFPS(30);
        FPS = 30;
        selectFPS.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }
    /////////////////////

    private void OnEnable()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    void LoadOptionData()
    {
        StringBuilder sb = new();
        sb.Append(Path);
        string loadFile = File.ReadAllText(sb.ToString());
        data = JsonUtility.FromJson<OptionSaveData>(loadFile);
    }

    void SaveOptionData()
    {
        StringBuilder sb = new();

        data.FPS = FPS;
        data.screenMode = screenMode;
        data.resolution = resolution;
        data.sensivity = sensivity;
        data.masterVolume = masterVolume;
        data.backgroundVolume = backgroundVolume;
        data.SFXVolume = SFXVolume;

        string saveFile = JsonUtility.ToJson(data, true);
        sb.Append(Path);
        File.WriteAllText(sb.ToString(), saveFile);
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        SaveOptionData();
    }
}
