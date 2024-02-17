using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCUI : MonoBehaviour
{
    public void ClickMainTitle()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        SceneLoader.Instance.LoadMainTitle();
        InterfaceMgr.Instance.ESCUI();
    }

    public void ClickOption()
    {
        SoundMgr.Instance.PlaySFXAudio("Click");
        InterfaceMgr.Instance.OptionUI();
    }

    public void ClickExit()
    {
        SoundMgr.Instance.PlaySFXAudio("Back");
        Application.Quit();
    }

    public void ClickBack()
    {
        SoundMgr.Instance.PlaySFXAudio("Back");
        InterfaceMgr.Instance.ESCUI();
    }

    private void OnEnable()
    {
        SoundMgr.Instance.PlaySFXAudio("UI_Open");
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
