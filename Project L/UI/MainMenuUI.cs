using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenuSelectUI; 
    [SerializeField] LoadUI loadUI;
    [SerializeField] OptionUI optionUI;

    public void Initialize()
    {
        gameObject.SetActive(true);

        mainMenuSelectUI.gameObject.SetActive(true);
        loadUI.Initalize();
        if(!File.Exists(optionUI.Path))
            optionUI.Initialize(false);
        else
            optionUI.Initialize(true);
    }

    public void ClickLoadUI()
    {
        loadUI.gameObject.SetActive(true);
        mainMenuSelectUI.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void ClickOptionUI()
    {
        optionUI.gameObject.SetActive(true);
        mainMenuSelectUI.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Click");
    }

    public void  BackOptionUI()
    {
        mainMenuSelectUI.SetActive(true);
        optionUI.gameObject.SetActive(false);
        SoundMgr.Instance.PlaySFXAudio("Back");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BackLoadUI()
    {
        loadUI.gameObject.SetActive(false);
        mainMenuSelectUI.SetActive(true);
        SoundMgr.Instance.PlaySFXAudio("Back");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClickNewGame()
    {
        SceneLoader.Instance.StartGame();
        SoundMgr.Instance.PlaySFXAudio("GameStart");
        gameObject.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        SoundMgr.Instance.PlaySFXAudio("Back");
        Application.Quit();
    }
}
